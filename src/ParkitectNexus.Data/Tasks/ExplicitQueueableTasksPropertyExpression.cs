using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace ParkitectNexus.Data.Tasks
{
    public class ExplicitQueueableTasksPropertyExpression
    {
        private readonly IQueueableTaskManager _queueableTaskManager;
        readonly IDictionary<string,object> _values = new  Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        internal ExplicitQueueableTasksPropertyExpression(IQueueableTaskManager queueableTaskManager)
        {
            _queueableTaskManager = queueableTaskManager;
        }

        public ExplicitQueueableTasksPropertyExpression With(string argName, object value)
        {
            if (argName == null) throw new ArgumentNullException(nameof(argName));

            _values[argName] = value;

            return this;
        }

        public void Add<TTask>() where TTask : IQueueableTask
        {
            var expression = _values.Aggregate<KeyValuePair<string, object>, ExplicitArgsExpression>(null,
                (current, keyValue) =>
                    current == null
                        ? ObjectFactory.Container.With((string) keyValue.Key).EqualTo(keyValue.Value)
                        : current.With(keyValue.Key).EqualTo(keyValue.Value));

            _queueableTaskManager.Add(expression.GetInstance<TTask>());
        }

        public void InsertAfter<TTask>(IQueueableTask afterTask) where TTask : IQueueableTask
        {
            var expression = _values.Aggregate<KeyValuePair<string, object>, ExplicitArgsExpression>(null,
                (current, keyValue) =>
                    current == null
                        ? ObjectFactory.Container.With(keyValue.Key).EqualTo(keyValue.Value)
                        : current.With(keyValue.Key).EqualTo(keyValue.Value));

            _queueableTaskManager.InsertAfter(expression.GetInstance<TTask>(), afterTask);
        }
    }
}