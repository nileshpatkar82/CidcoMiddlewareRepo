﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Cidco.Middleware.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Cidco.Middleware.Domain
{
    public partial interface ICidcoMiddlewareDBProcedures
    {
        Task<int> sp_InsertAapleUserAsync(string trackid, string userid, string UsertypeName, string UserName, string Password, string PasswordChanged, string EmailID, string Mobileno, string Fullname, string Fullname_mr, string Occupation, string Age, string gender, string DOB, string UIDNO, string PANNo, string pincode, string Divisionid, string Districtid, string Talukaid, string Villageid, string RegistrationType, string DistrictPrefix, string ServiceID, string UsrTimeStamp, string UsrSession, string ClientCheckSumValue, string strServiceCookie, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> sp_InsertEnquiryAsync(string appname, string mobileno, string email, string address, string module, string services, string comm, string mode, string enterby, decimal? Regcharge, string modeofpayment, bool? ispaymentdone, string appno, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
