using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using QuestSystem.BluePrints;
using QuestSystem.Utils;
using UnityEngine;

namespace QuestSystem.Resolvers
{
    public partial class ResolversMap
    {
        private QuestStepResolverProvider<BaseQuestStep, BaseQuestStepResolver> defaultResolver = new();

        private Dictionary<int, QuestStepResolverProvider> typeIndexToQuestStepResolver =
            new()
            {
                { IndexGenerator.GetIndexForType(typeof(CompositeLinearQuestStep)), new QuestStepResolverProvider<CompositeLinearQuestStep, CompositeLinearQuestStepResolver>() }
            };
        
        private Dictionary<Type, QuestStepResolverProvider> typeToQuestStepResolver =
            new()
            {
                { typeof(CompositeLinearQuestStep), new QuestStepResolverProvider<CompositeLinearQuestStep, CompositeLinearQuestStepResolver>() }
            };

        public void DeserializeQuestStepContainerToObject<T>(T value, QuestStepContainer container)
        {
            if (typeIndexToQuestStepResolver.TryGetValue(container.TypeIndex, out var provider))
            {
                provider.DeserializeJSONTo(value, container.Json);
            }
            else
            {
                defaultResolver.DeserializeJSONTo(value, container.Json);
            }
        }
        
        public string GetQuestStepResolverContainer<T>(T data)
        {
            var key = data.GetType();

            if (typeToQuestStepResolver.TryGetValue(key, out var provider))
            {
                var resolver = provider.SerializeToJSON(data);
                var container = new JSONContainer { JSON = resolver, TypeIndex = IndexGenerator.GetIndexForType(key) };

                var json = JsonConvert.SerializeObject(container);
                return json;
            }
            else
            {
                var resolver = defaultResolver.SerializeToJSON(data);
                var container = new JSONContainer { JSON = resolver, TypeIndex = IndexGenerator.GetIndexForType(key) };

                var json = JsonConvert.SerializeObject(container);
                return json;
            }

            return String.Empty;
        }
    }
    
    public abstract class QuestStepResolverProvider
    {
        public abstract string SerializeToJSON<T>(T data);
        public abstract void DeserializeJSONTo<T>(T obj, string data);
    }
    
    public class QuestStepResolverProvider<TypeToResolve, Resolver> : QuestStepResolverProvider where Resolver : struct, IQuestStepResolver<TypeToResolve, Resolver>
    {
        public readonly int TypeCode = IndexGenerator.GetIndexForType(typeof(TypeToResolve));

        public override void DeserializeJSONTo<T>(T obj, string data)
        {
            if (obj == null)
            {
                Debug.LogError("object is null");
                return;
            }

            var resolver = JsonConvert.DeserializeObject<Resolver>(data);

            if (obj is TypeToResolve result)
            {
                resolver.Out(ref result);
                return;
            }

            Debug.LogError($"we cant cast {typeof(TypeToResolve).Name} to {typeof(T).Name}");
        }

        //we return here new array, if we need non alloc realization, we should add new method with buffer 
        public override string SerializeToJSON<T>(T data)
        {
            if (data is TypeToResolve needed)
            {
                var resolver = new Resolver();
                resolver.In(ref needed);
                var result = JsonConvert.SerializeObject(resolver);
                return result;
            }

            Debug.LogError($"wrong serialization {typeof(TypeToResolve).Name} to {typeof(T).Name}");
            return default;
        }
    }

    [Serializable, JsonObject]
    public class QuestStepContainer
    {
        [JsonProperty("TypeIndex")]
        public int TypeIndex;
        [JsonProperty("Json")]
        public string Json;
    }
}