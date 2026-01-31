using System;
using System.Collections.Generic;
using System.Text;

namespace BehaviourAPI.BehaviourTrees
{
    using Core;
    using System.Diagnostics;
    using System.Reflection;
    using UnityEngine;

    /// <summary>
    /// Composite node that selects one of its branch to execute it.
    /// </summary>
    public abstract class BranchNode : CompositeNode
    {
     protected   BTNode m_SelectedNode;

        /// <summary>
        /// <inheritdoc/>
        /// Select a branch and starts it.
        /// </summary>
        public override void OnStarted()
        {
            base.OnStarted();

            int branchIndex = SelectBranchIndex();
            if (branchIndex < 0) branchIndex = 0;
            if (branchIndex >= ChildCount) branchIndex = ChildCount - 1;
            m_SelectedNode = GetBTChildAt(branchIndex);

            m_SelectedNode?.OnStarted();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Stops the selected branch node.
        /// </summary>
        public override void OnStopped()
        {
            base.OnStopped();
            m_SelectedNode?.OnStopped();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Pauses the selected branch node.
        /// </summary>
        public override void OnPaused()
        {
            base.OnPaused();
            m_SelectedNode?.OnPaused();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Unpauses the selected branch node.
        /// </summary>
        public override void OnUnpaused()
        {
            base.OnUnpaused();
            m_SelectedNode?.OnUnpaused();
        }

        /// <summary>
        /// <inheritdoc/>
        /// Returns the status of its selected branch.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected override Status UpdateStatus()
        {
            m_SelectedNode.OnUpdated();
            return m_SelectedNode?.Status ?? Status.Failure;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Override this method to define how to select the branch that will be executed.
        /// </summary>
        /// <returns><inheritdoc/></returns>
        protected abstract int SelectBranchIndex();
    }

    public abstract class ReactiveBranchNode : CompositeNode
    {
        int currentNode = -1;
        bool lastframeUpdated = false;
        protected BTNode m_SelectedNode;
        public int waitTimeFrames;
        bool wait;
      public  bool syncUpdate = false;
        int randomDelay;
        protected abstract int SelectBranchIndex();

        public override void OnStarted()
        {
            base.OnStarted();
            wait = waitTimeFrames > 0;
            randomDelay = syncUpdate ? 0 : Random.Range(0,10);
            lastframeUpdated = false;
            int branchIndex = SelectBranchIndex();
            if (branchIndex < 0) branchIndex = 0;
            if (branchIndex >= ChildCount) branchIndex = ChildCount - 1;
            m_SelectedNode = GetBTChildAt(branchIndex);
            currentNode = branchIndex;

            m_SelectedNode?.OnStarted();
            
        }
        protected override Status UpdateStatus()
        {
            if (!wait || (Time.frameCount + randomDelay) % waitTimeFrames == 0)
            {
                int branchIndex = SelectBranchIndex();
                if (currentNode != branchIndex)
                {
                    m_SelectedNode?.OnStopped();
                    if (branchIndex < 0) branchIndex = 0;
                    if (branchIndex >= ChildCount) branchIndex = ChildCount - 1;
                    currentNode = branchIndex;
                    m_SelectedNode = GetBTChildAt(branchIndex);
                    if (m_SelectedNode.Status == Status.None)
                        m_SelectedNode?.OnStarted();

                }
            }
            m_SelectedNode.OnUpdated();
            lastframeUpdated = true;
            return m_SelectedNode?.Status ?? Status.Failure;
            //return Status.Running;  
        }
        public override void OnPaused()
        {
            base.OnPaused();
            if (lastframeUpdated)
            {
                m_SelectedNode?.OnPaused();
            }
        }
        public override void OnStopped()
        {
            base.OnStopped();
            if (lastframeUpdated)
            {
                m_SelectedNode?.OnStopped();
            }
        }
        public override void OnUnpaused()
        {
            base.OnUnpaused();
            if (lastframeUpdated)
            {
                m_SelectedNode?.OnUnpaused();
            }
        }

    }
}
