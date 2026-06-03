using UnityEngine;
using UnityEditor;

public class FixDeathZoneAndBallValues
{
    public static void Execute()
    {
        // Fix DeathZone — top edge stays at y=-6.4, height grows to 2.0 so center moves to y=-7.4
        var deathZoneGO = GameObject.Find("DeathZone");
        if (deathZoneGO != null)
        {
            var col = deathZoneGO.GetComponent<BoxCollider2D>();
            if (col != null)
            {
                var so = new SerializedObject(col);
                so.FindProperty("m_Size").vector2Value = new Vector2(21.8f, 2.0f);
                so.ApplyModifiedProperties();
            }
            var t = new SerializedObject(deathZoneGO.transform);
            t.FindProperty("m_LocalPosition").vector3Value = new Vector3(0f, -7.4f, 0f);
            t.ApplyModifiedProperties();
            EditorUtility.SetDirty(deathZoneGO);
        }
        else
        {
            Debug.LogError("FixDeathZoneAndBallValues: DeathZone not found.");
        }

        // Fix Ball — re-apply tuned values and lock IsMain = true
        var ballGO = GameObject.Find("Ball");
        if (ballGO != null)
        {
            var ball = ballGO.GetComponent<BallController>();
            if (ball != null)
            {
                var so = new SerializedObject(ball);
                so.FindProperty("BaseSpeed").floatValue  = 11f;
                so.FindProperty("_minSpeed").floatValue  = 8f;
                so.FindProperty("IsMain").boolValue      = true;
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(ball);
            }
            else
            {
                Debug.LogError("FixDeathZoneAndBallValues: BallController not found on Ball.");
            }
        }
        else
        {
            Debug.LogError("FixDeathZoneAndBallValues: Ball not found.");
        }

        Debug.Log("FixDeathZoneAndBallValues: All fixes applied.");
    }
}
