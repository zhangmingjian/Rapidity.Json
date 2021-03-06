﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Rapidity.Json
{
    internal class Stack<T> : IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ICollection
    {
        public const int DefaultCapacity = 4;
        private T[] _array;
        private int _index;
        public int Count => _index + 1;

        public bool IsSynchronized => _array.IsSynchronized;

        public object SyncRoot => _array.SyncRoot;

        public Stack() : this(DefaultCapacity)
        {
        }

        public Stack(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException(nameof(capacity));
            _array = new T[capacity];
            _index = -1;
        }

        public void Push(T data)
        {
            if (_index + 1 >= _array.Length)
            {
                Array.Resize(ref _array, _array.Length == 0 ? DefaultCapacity : _array.Length * 2);
            }
            _array[++_index] = data;
        }

        public T Pop()
        {
            if (Count <= 0) throw new Exception("当前栈为空");
            var value = _array[_index];
            _array[_index] = default;
            _index--;
            return value;
        }

        public bool TryPop(out T value)
        {
            if (Count <= 0)
            {
                value = default;
                return false;
            }
            value = _array[_index];
            _array[_index] = default;
            _index--;
            return true;
        }

        public T Peek()
        {
            if (Count <= 0) throw new Exception("当前栈为空");
            return _array[_index];
        }

        public bool TryPeek(out T value)
        {
            if (Count <= 0)
            {
                value = default;
                return false;
            }
            value = _array[_index];
            return true;
        }

        public void Clear()
        {
            Array.Clear(_array, 0, Count);
            _index = -1;
        }

        public bool Contains(T item)
        {
            return Count != 0 && Array.LastIndexOf(_array, item, _index) != -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            if (Count == 0) return Array.Empty<T>();
            var array = new T[Count];
            CopyTo(array, 0);
            return array;
        }

        public void CopyTo(Array array, int index)
        {
            if (Count == 0) return;
            for (int i = _index; i >= 0; i--)
            {
                array.SetValue(_array[i], index++);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new StackEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct StackEnumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            private Stack<T> _stack;
            private T _current;
            public T Current => _current;
            object IEnumerator.Current => _current;
            private int _index;

            public StackEnumerator(Stack<T> stack)
            {
                _stack = stack;
                _index = _stack._index;
                _current = default;
            }

            public bool MoveNext()
            {
                if (_index <= -1) return false;
                _current = _stack._array[_index--];
                return true;
            }

            public void Reset()
            {
                _index = _stack._index;
            }

            public void Dispose()
            {
            }
        }
    }
}