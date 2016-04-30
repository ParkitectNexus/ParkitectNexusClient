// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
            _queueableTaskManager.InsertAfter(_expression.GetInstance<TTask>(), afterTask);
        }
    }
}