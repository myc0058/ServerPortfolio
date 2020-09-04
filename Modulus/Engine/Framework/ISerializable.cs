using System;
namespace Engine.Framework
{
    public interface ISerializable
    {
        void Serialize(System.IO.Stream output);
        int Length { get; }
    }
}
