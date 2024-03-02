﻿using BuildingBlock.Base.Models.Base;

namespace BasketService.Domain.Aggregate.ValueObjects
{
    public sealed class UserId : ValueObject
    {
        public Guid Id { get; }

        public UserId(Guid id)
        {
            Id = id;
        }

        public static UserId CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static UserId Create(Guid Id)
        {
            return new UserId(Id);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }
    }
}
