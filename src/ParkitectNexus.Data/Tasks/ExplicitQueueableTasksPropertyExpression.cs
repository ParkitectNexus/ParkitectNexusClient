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
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace ParkitectNexus.Data.Tasks
{
    public class ExplicitQueueableTasksPropertyExpression
    {
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
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
                        ? ObjectFactory.Container.With(keyValue.Key).EqualTo(keyValue.Value)
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