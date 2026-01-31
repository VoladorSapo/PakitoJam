namespace BehaviourAPI.UtilitySystems
{
    /// <summary>
    /// Create a curve factor with a linear function
    /// </summary>
    public class LinearCurveFactor : CurveFactor
    {
        /// <summary>
        /// The slope of the function.
        /// </summary>
        public float Slope = 1f;

        /// <summary>
        /// The y intercept of the function.
        /// </summary>
        public float YIntercept = 0f;

        /// <summary>
        /// Set the slope of the linear curve factor.
        /// </summary>
        /// <param name="slope">The new slope.</param>
        /// <returns>The <see cref="LinearCurveFactor"/> itself. </returns>
        public LinearCurveFactor SetSlope(float slope)
        {
            this.Slope = slope;
            return this;
        }

        /// <summary>
        /// Set the y intercept of the linear curve factor.
        /// </summary>
        /// <param name="yIntercept">The new y intercept.</param>
        /// <returns>The <see cref="LinearCurveFactor"/> itself. </returns>
        public LinearCurveFactor SetYIntercept(float yIntercept)
        {
            this.YIntercept = yIntercept;
            return this;
        }

        /// <summary>
        /// Compute the utility using a linear function [y = slope * x + yIntercept]
        /// </summary>
        /// <param name="x">The child utility. </param>
        /// <returns>The result of apply the function to <paramref name="x"/>.</returns>
        protected override float Evaluate(float x) => Slope * x + YIntercept;
    }


    public class ThresholdCurveFactor : CurveFactor
    {
        /// <summary>
        /// The slope of the function.
        /// </summary>
        public float Threshold = 0.5f;

        /// <summary>
        /// The y intercept of the function.
        /// </summary>

        /// <summary>
        /// Set the slope of the linear curve factor.
        /// </summary>
        /// <param name="slope">The new slope.</param>
        /// <returns>The <see cref="LinearCurveFactor"/> itself. </returns>
        public ThresholdCurveFactor SetThreshold(float slope)
        {
            this.Threshold = slope;
            return this;
        }

        public bool Inverted = false;

        public ThresholdCurveFactor SetInverted(bool inverted)
        {
            this.Inverted = inverted;
            return this;
        }

        /// <summary>
        /// Compute the utility using a linear function [y = slope * x + yIntercept]
        /// </summary>
        /// <param name="x">The child utility. </param>
        /// <returns>The result of apply the function to <paramref name="x"/>.</returns>
        protected override float Evaluate(float x){

            if (!Inverted)
            {
                return x >= Threshold ? 1 : 0;
            }
            else
            {
                return x <= Threshold ? 1 : 0;
            }

        }
    }
}
