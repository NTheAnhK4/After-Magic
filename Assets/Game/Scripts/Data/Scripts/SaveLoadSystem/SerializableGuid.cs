using System;
using UnityEngine;

[Serializable]
public sealed class SerializableGuid : IComparable,
    IComparable<SerializableGuid>,
    IComparable<Guid>,
    IEquatable<SerializableGuid>,
    IEquatable<Guid>,
    ISerializationCallbackReceiver
{
    [SerializeField] private byte[] serializedGuid;
    private Guid guid;

    public SerializableGuid(Guid guid)
    {
        this.guid = guid;
        serializedGuid = guid.ToByteArray();
    }

    public SerializableGuid()
    {
        guid = Guid.NewGuid();
        serializedGuid = guid.ToByteArray();
    }

    public void OnBeforeSerialize()
    {
        if (guid == Guid.Empty)
        {
            guid = Guid.NewGuid();
        }
        serializedGuid = guid.ToByteArray();
    }

    public void OnAfterDeserialize()
    {
        if (serializedGuid != null && serializedGuid.Length == 16)
        {
            guid = new Guid(serializedGuid);
        }
        else
        {
            guid = Guid.NewGuid(); // fallback
            serializedGuid = guid.ToByteArray();
        }
    }

    public bool Equals(SerializableGuid other) => guid.Equals(other.guid);
    public bool Equals(Guid other) => guid.Equals(other);
    public override bool Equals(object obj) =>
        obj switch
        {
            SerializableGuid sg => sg.guid.Equals(guid),
            Guid g => g.Equals(guid),
            _ => false
        };

    public int CompareTo(SerializableGuid other) => guid.CompareTo(other.guid);
    public int CompareTo(Guid other) => guid.CompareTo(other);
    public int CompareTo(object obj) =>
        obj switch
        {
            SerializableGuid sg => sg.guid.CompareTo(guid),
            Guid g => g.CompareTo(guid),
            _ => -1
        };

    public override int GetHashCode() => HashCode.Combine(guid);
    public override string ToString() => guid.ToString();

    public byte[] ToByteArray() => guid.ToByteArray();

    public static bool operator ==(SerializableGuid a, SerializableGuid b) => a.Equals(b);
    public static bool operator !=(SerializableGuid a, SerializableGuid b) => !a.Equals(b);

    public static implicit operator SerializableGuid(Guid guid) => new(guid);
    public static implicit operator Guid(SerializableGuid serializable) => serializable.guid;
}
