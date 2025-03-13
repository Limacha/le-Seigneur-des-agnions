using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment : MonoBehaviour
{
    [TextArea(10, 20)]
    [SerializeField] private string commentText;
    public string CommentText { get { return commentText; } }
}
