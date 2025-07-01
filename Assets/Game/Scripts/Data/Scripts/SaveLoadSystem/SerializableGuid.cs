using System;
using UnityEngine;

[Serializable]
public sealed class SerializableGuid : IComparable,
    IComparable<SerializableGuid>,
    IComparable<Guid>,
    IEquatable<SerializableGuid>,
    IEquatable<Guid>,
    ISerializationCallbackReceiver {

    [SerializeField]
    private byte[] serializedGuid;
    private Guid guid;

    public SerializableGuid(Guid guid) {
        this.guid = guid;
        serializedGuid = guid.ToByteArray();
    }
    public SerializableGuid() {
        guid = Guid.NewGuid();
        serializedGuid = guid.ToByteArray();
    }


    public bool Equals(SerializableGuid other) => guid.Equals(other.guid);
    public bool Equals(Guid other) => guid.Equals(other);
    public override bool Equals(object obj) {
        if (obj is SerializableGuid serializableGuid) {
            return serializableGuid.guid.Equals(this.guid);
        }
        if (obj is Guid guid) {
            return guid.Equals(this.guid);
        }
        return false;
    }

    public int CompareTo(SerializableGuid other) => guid.CompareTo(other.guid);
    public int CompareTo(Guid other) => guid.CompareTo(other);
    public int CompareTo(object obj) {
        if (obj is SerializableGuid serializableGuid) {
            return serializableGuid.guid.CompareTo(this.guid);
        }
        if (obj is Guid guid) {
            return guid.CompareTo(this.guid);
        }
        return -1;
    }

    public override int GetHashCode() {
        return HashCode.Combine(guid);
    }

    public override string ToString() => guid.ToString();
    public string ToString(string format) => guid.ToString(format);
    public string ToString(string format, IFormatProvider provider) => guid.ToString(format, provider);

    public byte[] ToByteArray() => guid.ToByteArray();

    public void OnAfterDeserialize() {
        if (guid != Guid.Empty) {
            serializedGuid = guid.ToByteArray();
        }
    }

    public void OnBeforeSerialize() {
        if (serializedGuid == null || serializedGuid.Length != 16) {
            serializedGuid = new byte[16];
        }
        guid = new Guid(serializedGuid);
    }

    public static bool operator ==(SerializableGuid a, SerializableGuid b) => a.Equals(b);
    public static bool operator !=(SerializableGuid a, SerializableGuid b) => !a.Equals(b);

    public static implicit operator SerializableGuid(Guid guid) => new(guid);
    public static implicit operator Guid(SerializableGuid serializable) => serializable.guid;
}