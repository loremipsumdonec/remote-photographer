namespace RemotePhotographerTest.Services;

public class PhotographerFixture
{
    private readonly RemotePhotographerEngine _engine;

    public PhotographerFixture(RemotePhotographerEngine engine)
    {
        _engine = engine;
        _engine.Start();
    }
    public T GetService<T>()
    {
        return (T)_engine.Services.GetService(typeof(T));
    }
}

