using UnityEditor;

namespace Assets
{
    [CustomEditor(typeof(TwitchClient))]
    public class TwitchClientEditor : Editor
    {
        SerializedProperty defaultChannel;
        SerializedProperty defaultUser;
        SerializedProperty defaultToken;
        private string pw;
        void OnEnable()
        {
            defaultChannel = serializedObject.FindProperty("defaultChannel");
            defaultUser = serializedObject.FindProperty("defaultUser");
            defaultToken = serializedObject.FindProperty("defaultToken");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(defaultChannel);
            EditorGUILayout.PropertyField(defaultUser);
            
            // Hide the oauth token from the stream
            //EditorGUILayout.PropertyField(defaultToken);

            EditorGUILayout.LabelField("OAuth Access Token");
            var rect = EditorGUILayout.GetControlRect();
            EditorGUI.BeginProperty(rect, null, defaultToken);
            defaultToken.stringValue = EditorGUILayout.PasswordField(defaultToken.stringValue);
            EditorGUI.EndProperty();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
