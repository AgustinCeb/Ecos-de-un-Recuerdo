using System;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class QuestIDList : INetworkSerializable
{
    public string[] IDs;

    public QuestIDList()
    {
        IDs = Array.Empty<string>();
    }

    public QuestIDList(string[] ids)
    {
        IDs = ids ?? Array.Empty<string>();
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer)
        where T : IReaderWriter
    {
        int count = IDs == null ? 0 : IDs.Length;

        serializer.SerializeValue(ref count);

        if (serializer.IsReader)
            IDs = new string[count];

        for (int i = 0; i < count; i++)
            serializer.SerializeValue(ref IDs[i]);
    }
}