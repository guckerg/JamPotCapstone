namespace JampotUnitTests.Helpers
{
    internal class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStore = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;

        public string Id => "test-session-id"; // Provide a dummy ID

        public IEnumerable<string> Keys => _sessionStore.Keys;

        public void Clear()
        {
            _sessionStore.Clear();
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync()
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            _sessionStore.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionStore[key] = value;
        }

        public bool TryGetValue(string key, out byte[]? value)
        {
            return _sessionStore.TryGetValue(key, out value);
        }
    }
}
