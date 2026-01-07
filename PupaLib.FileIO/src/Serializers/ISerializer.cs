namespace PupaLib.FileIO.Serializers;

public interface ISerializer {
  public byte[] Serialize<T>(T content) where T : class;
  public T Deserialize<T>(byte[] bytes) where T : class;
}