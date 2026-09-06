using System;
using System.Collections.Generic;

namespace YgoAiPlatform.Core
{
    public class MlpLayer
    {
        public double[][] Weights { get; set; } = Array.Empty<double[]>(); // [InputSize][OutputSize]
        public double[] Biases { get; set; } = Array.Empty<double>();
    }

    public class MlpModel
    {
        public List<MlpLayer> Layers { get; set; } = new();

        public double Compute(double[] features)
        {
            if (Layers == null || Layers.Count == 0)
                return 0.0;

            double[] current = features;
            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                if (layer.Weights == null || layer.Biases == null)
                    return 0.0;

                int inputSize = current.Length;
                int outputSize = layer.Biases.Length;
                double[] next = new double[outputSize];

                for (int outIdx = 0; outIdx < outputSize; outIdx++)
                {
                    double sum = layer.Biases[outIdx];
                    for (int inIdx = 0; inIdx < Math.Min(inputSize, layer.Weights.Length); inIdx++)
                    {
                        if (layer.Weights[inIdx] != null && outIdx < layer.Weights[inIdx].Length)
                        {
                            sum += current[inIdx] * layer.Weights[inIdx][outIdx];
                        }
                    }
                    if (i < Layers.Count - 1)
                    {
                        next[outIdx] = Math.Tanh(sum);
                    }
                    else
                    {
                        next[outIdx] = sum;
                    }
                }
                current = next;
            }

            return current.Length > 0 ? current[0] : 0.0;
        }
    }
}
