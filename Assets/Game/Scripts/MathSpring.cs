using UnityEngine;

public struct DampedSpringMotionParams
{
    public float m_posPosCoef;
    public float m_posVelCoef;
    public float m_velPosCoef;
    public float m_velVelCoef;
}

public static class MathSpring
{
    const float epsilon = 0.0001f;

    public static void CalculateSpringParams(
        ref DampedSpringMotionParams outParams,
        float deltaTime,
        float angularFrequency,
        float dampingRatio)
    {
        dampingRatio = Mathf.Max(dampingRatio, 0f);
        angularFrequency = Mathf.Max(angularFrequency, 0f);

        if (angularFrequency < epsilon)
        {
            outParams = new DampedSpringMotionParams
            {
                m_posPosCoef = 1f,
                m_posVelCoef = 0f,
                m_velPosCoef = 0f,
                m_velVelCoef = 1f
            };
            return;
        }

        if (dampingRatio > 1f + epsilon)
        {
            float za = -angularFrequency * dampingRatio;
            float zb = angularFrequency * Mathf.Sqrt(dampingRatio * dampingRatio - 1f);
            float z1 = za - zb;
            float z2 = za + zb;

            float e1 = Mathf.Exp(z1 * deltaTime);
            float e2 = Mathf.Exp(z2 * deltaTime);
            float invTwoZb = 1f / (2f * zb);

            float e1_over_two_zb = e1 * invTwoZb;
            float e2_over_two_zb = e2 * invTwoZb;
            float z1e1_over_two_zb = z1 * e1_over_two_zb;
            float z2e2_over_two_zb = z2 * e2_over_two_zb;

            outParams.m_posPosCoef = e1_over_two_zb * z2 - z2e2_over_two_zb + e2;
            outParams.m_posVelCoef = -e1_over_two_zb + e2_over_two_zb;
            outParams.m_velPosCoef = (z1e1_over_two_zb - z2e2_over_two_zb + e2) * z2;
            outParams.m_velVelCoef = -z1e1_over_two_zb + z2e2_over_two_zb;
        }
        else if (dampingRatio < 1f - epsilon)
        {
            float omegaZeta = angularFrequency * dampingRatio;
            float alpha = angularFrequency * Mathf.Sqrt(1f - dampingRatio * dampingRatio);
            float expTerm = Mathf.Exp(-omegaZeta * deltaTime);
            float cosTerm = Mathf.Cos(alpha * deltaTime);
            float sinTerm = Mathf.Sin(alpha * deltaTime);
            float invAlpha = 1f / alpha;

            float expSin = expTerm * sinTerm;
            float expCos = expTerm * cosTerm;
            float expOmegaZetaSin_Over_Alpha = expTerm * omegaZeta * sinTerm * invAlpha;

            outParams.m_posPosCoef = expCos + expOmegaZetaSin_Over_Alpha;
            outParams.m_posVelCoef = expSin * invAlpha;
            outParams.m_velPosCoef = -expSin * alpha - omegaZeta * expOmegaZetaSin_Over_Alpha;
            outParams.m_velVelCoef = expCos - expOmegaZetaSin_Over_Alpha;
        }
        else
        {
            float expTerm = Mathf.Exp(-angularFrequency * deltaTime);
            float timeExp = deltaTime * expTerm;
            float timeExpFreq = timeExp * angularFrequency;

            outParams.m_posPosCoef = timeExpFreq + expTerm;
            outParams.m_posVelCoef = timeExp;
            outParams.m_velPosCoef = -angularFrequency * timeExpFreq;
            outParams.m_velVelCoef = -timeExpFreq + expTerm;
        }
    }

    public static void UpdateSpring(
        ref float position,
        ref float velocity,
        float target,
        ref DampedSpringMotionParams param)
    {
        float oldPos = position - target;
        position = oldPos * param.m_posPosCoef + velocity * param.m_posVelCoef + target;
        velocity = oldPos * param.m_velPosCoef + velocity * param.m_velVelCoef;
    }

    public static void UpdateSpring(
        ref Vector2 position,
        ref Vector2 velocity,
        Vector2 target,
        ref DampedSpringMotionParams param)
    {
        Vector2 oldPos = position - target;
        position = oldPos * param.m_posPosCoef + velocity * param.m_posVelCoef + target;
        velocity = oldPos * param.m_velPosCoef + velocity * param.m_velVelCoef;
    }

    public static void UpdateSpring(
        ref Vector3 position,
        ref Vector3 velocity,
        Vector3 target,
        ref DampedSpringMotionParams param)
    {
        Vector3 oldPos = position - target;
        position = oldPos * param.m_posPosCoef + velocity * param.m_posVelCoef + target;
        velocity = oldPos * param.m_velPosCoef + velocity * param.m_velVelCoef;
    }
}
