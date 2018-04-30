﻿using System.Collections.Generic;
using Adnc.FluidBT.Tasks;

namespace Adnc.FluidBT.TaskParents {
    public abstract class TaskParentBase : ITaskParent {
        public AbortType AbortType { get; set; } = AbortType.None;

        public bool Enabled { get; set; } = true;

        public List<ITask> children { get; } = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;

        public bool IsLowerPriority => AbortType.HasFlag(AbortType.LowerPriority);
        public bool ValidAbortCondition { get; } = false;

        public TaskStatus Update () {
            return OnUpdate();
        }

        // @TODO Should allow returning multiple conditions (for selectors)
        // @TODO Should only return a valid condition type (skips non conditions)
        public ITask GetAbortCondition () {
            if (children.Count > 0 && children[0].ValidAbortCondition) {
                return children[0];
            }

            return null;
        }

        public void End () {
            throw new System.NotImplementedException();
        }

        protected virtual TaskStatus OnUpdate () {
            return TaskStatus.Success;
        }

        public virtual void Reset (bool hardReset = false) {
            if (children.Count <= 0) return;
            foreach (var child in children) {
                child.Reset(hardReset);
            }
        }

        public void AddChild (ITask child) {
            if (children.Count < MaxChildren || MaxChildren < 0) {
                children.Add(child);
            }
        }
    }
}