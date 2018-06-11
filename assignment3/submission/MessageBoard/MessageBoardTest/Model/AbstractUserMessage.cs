using System;
using System.Collections.Generic;


public class AbstractUserMessageList : List<AbstractUserMessage>
{
    /// <summary>
    /// Can be use to store if model has received an acknowledge
    /// </summary>
    public bool HasReceivedAck { get; set; }

    public AbstractUserMessageList()
    {
        HasReceivedAck = false;
    }
}

/// <summary>
/// Abstract version of user messages. 
/// It does not use IDs, because they are not relevant 
/// for modeling. 
/// </summary>
/// 
public class AbstractUserMessage
{
    public string Author { get; set; }
    public string Message { get; set; }
    // please note, that Sequence is an immutable data structure
    // so if you want to update the likes associated with a user message umd
    // you need to do something like umd.Likes = umd.Likes.Add("some like")
    // because umd.Likes.Add returns an updated sequence, rather than updating the 
    // sequence umd.Likes in place
    public List<string> Likes { get; set; }
    public AbstractUserMessage()
    {
        Author = "";
        Message = "";
        Likes = new List<string>();
    }

    /// <summary>
    /// ToString()-method, defined in the same way as for concrete
    /// user messages.
    /// </summary>
    /// <returns>string representation of abstract user message</returns>
    public override string ToString()
    {
        return Author + ":" + Message + " liked by :" + String.Join(",", Likes);
    }
}