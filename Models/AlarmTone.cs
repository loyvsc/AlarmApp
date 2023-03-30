﻿using PropertyChanged;
using Realms;
using System;

namespace AlarmApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class AlarmTone : RealmObject
    {
        [PrimaryKey] public string Id { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsCustomTone { get; set; }

        public AlarmTone()
        {
            Id = Guid.NewGuid().ToString();
        }

        public AlarmTone(string name, string path) : this()
        {
            Name = name;
            Path = path;
            IsCustomTone = false;
        }

        public override bool Equals(object obj)
        {
            if (this is null || obj is null)
            {
                return false;
            }

            if (!(obj is AlarmTone))
            {
                return false;
            }

            AlarmTone otherTone = obj as AlarmTone;
            if (Id == otherTone.Id)
            {
                return true;
            }

            if (Name == otherTone.Name && Path == otherTone.Path)
            {
                return true;
            }
            return false;
        }
    }
}