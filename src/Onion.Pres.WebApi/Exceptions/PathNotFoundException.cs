﻿using Onion.Shared.Exceptions;

namespace Onion.Pres.WebApi.Exceptions;

public class PathNotFoundException : NotFoundException
{
    private const string MESSAGE_KEY = "PathNotFound";
    public PathNotFoundException() : base(MESSAGE_KEY)
    {
    }
}