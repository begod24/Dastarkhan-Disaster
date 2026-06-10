using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [SerializeField] private List<RecipeSO> _availableRecipes = new();
    [SerializeField] private List<PlatedRecipeSO> _platedRecipes = new();
    [SerializeField] private float _spawnInterval = 15f;
    [SerializeField] private int _maxConcurrentOrders = 3;
    [SerializeField] private float _initialDelay = 3f;
    [SerializeField] private bool _autoSpawn = true;

    private readonly List<ActiveOrder> _activeOrders = new();
    private float _spawnTimer;
    private int _nextOrderId = 1;
    private bool _spawningEnabled;
    private bool _sessionEnded;

    public IReadOnlyList<ActiveOrder> ActiveOrders => _activeOrders;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<SessionEndedEvent>(OnSessionEnded);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<SessionEndedEvent>(OnSessionEnded);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void Start()
    {
        _spawnTimer = _initialDelay;
        _spawningEnabled = _autoSpawn;
    }

    public void SetSpawningEnabled(bool enabled) => _spawningEnabled = enabled;

    public void ClearAllOrders()
    {
        _activeOrders.Clear();
        _spawnTimer = _initialDelay;
    }

    private void OnSessionEnded(SessionEndedEvent e)
    {
        _sessionEnded = true;
        _spawningEnabled = false;
    }

    private void Update()
    {
        if (_sessionEnded) return;
        TickTimers();
        HandleSpawning();
    }

    private void TickTimers()
    {
        for (int i = _activeOrders.Count - 1; i >= 0; i--)
        {
            var order = _activeOrders[i];
            if (order.State != OrderState.Pending) continue;

            order.TimeRemaining -= Time.deltaTime;
            EventBus.Raise(new OrderTimerUpdatedEvent { Order = order });

            if (order.TimeRemaining <= 0f)
            {
                order.TimeRemaining = 0f;
                order.State = OrderState.Expired;
                EventBus.Raise(new OrderExpiredEvent { Order = order });
                _activeOrders.RemoveAt(i);
            }
        }
    }

    private void HandleSpawning()
    {
        if (!_spawningEnabled) return;
        if (_availableRecipes.Count + _platedRecipes.Count == 0) return;
        if (_activeOrders.Count >= _maxConcurrentOrders) return;

        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer > 0f) return;

        SpawnRandomOrder();
        _spawnTimer = _spawnInterval;
    }

    public ActiveOrder SpawnRandomOrder()
    {
        int total = _availableRecipes.Count + _platedRecipes.Count;
        if (total == 0) return null;
        int pick = Random.Range(0, total);
        if (pick < _availableRecipes.Count) return SpawnOrder(_availableRecipes[pick]);
        return SpawnOrder(_platedRecipes[pick - _availableRecipes.Count]);
    }

    public ActiveOrder SpawnOrder(RecipeSO recipe)
    {
        if (recipe == null) return null;

        var order = new ActiveOrder
        {
            Id = _nextOrderId++,
            Recipe = recipe,
            TimeLimit = recipe.TimeLimit,
            TimeRemaining = recipe.TimeLimit,
            State = OrderState.Pending
        };

        _activeOrders.Add(order);
        EventBus.Raise(new OrderSpawnedEvent { Order = order });
        return order;
    }

    public ActiveOrder SpawnOrder(PlatedRecipeSO recipe)
    {
        if (recipe == null) return null;

        var order = new ActiveOrder
        {
            Id = _nextOrderId++,
            PlatedRecipe = recipe,
            TimeLimit = recipe.TimeLimit,
            TimeRemaining = recipe.TimeLimit,
            State = OrderState.Pending
        };

        _activeOrders.Add(order);
        EventBus.Raise(new OrderSpawnedEvent { Order = order });
        return order;
    }

    public bool TryDeliver(Ingredient item, out ActiveOrder matched, out int score)
    {
        matched = null;
        score = 0;
        if (item == null) return false;
        if (_sessionEnded) return false;

        ActiveOrder best = null;
        for (int i = 0; i < _activeOrders.Count; i++)
        {
            var o = _activeOrders[i];
            if (o.State != OrderState.Pending) continue;
            if (o.Recipe == null) continue;
            if (!o.Recipe.Matches(item)) continue;
            if (best == null || o.TimeRemaining < best.TimeRemaining) best = o;
        }

        if (best == null) return false;

        int speedBonus = Mathf.RoundToInt(best.TimeRemaining * 10f);
        score = best.BaseScore + speedBonus;

        best.State = OrderState.Delivered;
        _activeOrders.Remove(best);

        matched = best;
        EventBus.Raise(new OrderCompletedEvent { Order = best, Score = score });
        return true;
    }

    public bool TryDeliver(Plate plate, out ActiveOrder matched, out int score)
    {
        matched = null;
        score = 0;
        if (plate == null) return false;
        if (_sessionEnded) return false;

        ActiveOrder best = null;
        for (int i = 0; i < _activeOrders.Count; i++)
        {
            var o = _activeOrders[i];
            if (o.State != OrderState.Pending) continue;
            if (o.PlatedRecipe == null) continue;
            if (!o.PlatedRecipe.Matches(plate.Contents)) continue;
            if (best == null || o.TimeRemaining < best.TimeRemaining) best = o;
        }

        if (best == null) return false;

        int speedBonus = Mathf.RoundToInt(best.TimeRemaining * 10f);
        score = best.BaseScore + speedBonus;

        best.State = OrderState.Delivered;
        _activeOrders.Remove(best);

        matched = best;
        EventBus.Raise(new OrderCompletedEvent { Order = best, Score = score });
        return true;
    }
}