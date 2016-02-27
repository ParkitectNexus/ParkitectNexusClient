// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using StructureMap;

namespace ParkitectNexus.Data.Tasks
{
    public class ExplicitQueueableTaskArgsExpression
    {
        private readonly ExplicitArgsExpression _expression;
        private readonly IQueueableTaskManager _queueableTaskManager;

        internal ExplicitQueueableTaskArgsExpression(IQueueableTaskManager queueableTaskManager,
            ExplicitArgsExpression expression)
        {
            if (queueableTaskManager == null) throw new ArgumentNullException(nameof(queueableTaskManager));
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            _queueableTaskManager = queueableTaskManager;
            _expression = expression;
        }

        public ExplicitQueueableTaskArgsExpression With<TArg>(TArg arg)
        {
            var expression = ObjectFactory.Container.With(arg);
            return new ExplicitQueueableTaskArgsExpression(_queueableTaskManager, expression);
        }

        public void Add<TTask>() where TTask : IQueueableTask
        {
            _queueableTaskManager.Add(_expression.GetInstance<TTask>());
        }

        public void InsertAfter<TTask>(IQueueableTask afterTask) where TTask : IQueueableTask
        {
            _queueableTaskManager.InsertAfter<TTask>(afterTask);
        }
    }
}
