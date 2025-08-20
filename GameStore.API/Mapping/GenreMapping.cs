using System;
using GameStore.API.Entities;

namespace GameStore.API.Mapping;

public static class GenreMapping
{
public static GenreDto ToDto(this Genre genre)
    {
        if (genre is null) throw new ArgumentNullException(nameof(genre));

        return new GenreDto(genre.Id, genre.Name);
    }
}
