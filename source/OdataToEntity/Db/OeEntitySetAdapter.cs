﻿using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace OdataToEntity.Db
{
    public abstract class OeEntitySetAdapter
    {
        public abstract void AddEntity(Object dataContext, ODataResourceBase entry);
        public abstract void AttachEntity(Object dataContext, ODataResourceBase entry);
        public abstract IQueryable GetEntitySet(Object dataContext);
        public abstract void RemoveEntity(Object dataContext, ODataResourceBase entry);

        public abstract Type EntityType { get; }
        public abstract String EntitySetName { get; }
        public virtual bool IsDbQuery => false;
    }

    public sealed class OeEntitySetAdapterCollection : ReadOnlyCollection<OeEntitySetAdapter>
    {
        public OeEntitySetAdapterCollection(OeEntitySetAdapter[] entitySetAdapters) : base(entitySetAdapters)
        {
        }

        public OeEntitySetAdapter Find(Type clrType)
        {
            var entitySetAdapters = (OeEntitySetAdapter[])base.Items;
            foreach (OeEntitySetAdapter entitySetAdapter in entitySetAdapters)
                if (entitySetAdapter.EntityType == clrType && !entitySetAdapter.IsDbQuery)
                    return entitySetAdapter;

            return null;
        }
        public OeEntitySetAdapter Find(IEdmEntitySet entitySet)
        {
            var entitySetAdapters = (OeEntitySetAdapter[])base.Items;
            foreach (OeEntitySetAdapter entitySetAdapter in entitySetAdapters)
                if (String.Compare(entitySetAdapter.EntitySetName, entitySet.Name, StringComparison.OrdinalIgnoreCase) == 0)
                    return entitySetAdapter;

            return null;
        }
    }
}
