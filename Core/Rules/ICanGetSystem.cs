﻿
namespace Framework
{
    public interface ICanGetSystem : IBelongArchiecture
    {
    }

    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
}
