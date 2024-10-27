﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Test.Application.DTOs.User
{
    public record UserUpdateDTO
    (
         int UserId,
         string RoleId
    );
}