﻿/**
 * @license
 * Internet Systems Consortium license
 *
 * Copyright (c) 2020 Maksym Sadovnychyy (MAKS-IT)
 * Website: https://maks-it.com
 * Email: commercial@maks-it.com
 *
 * Permission to use, copy, modify, and/or distribute this software for any purpose
 * with or without fee is hereby granted, provided that the above copyright notice
 * and this permission notice appear in all copies.
 *
 * THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH
 * REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY SPECIAL, DIRECT,
 * INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM LOSS
 * OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER
 * TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF
 * THIS SOFTWARE.
 */

using System;
using System.Management.Automation;
using System.Data;
using Lib;

namespace PSCmdlets
{
    [Cmdlet(VerbsLifecycle.Invoke, "MSSQLQuery")]
    [OutputType(typeof(PSObject[]))]
    public class InvokeMSSQLQuery : Cmdlet
    {
        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string ConnectionString { get; set; }

        [Parameter(Position = 2, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string Statement { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            try
            {
                DataTable queryResult = CMSQLClient.ExecuteQuery(ConnectionString, Statement);
                foreach (DataRow row in queryResult.Rows)
                {
                    PSObject obj = new PSObject();

                    for (int i = 0; i < queryResult.Columns.Count; i++)
                    {
                        obj.Members.Add(new PSNoteProperty(queryResult.Columns[i].ColumnName, row[i].ToString()));
                    }

                    WriteObject(obj);
                }
            }
            catch (Exception ex)
            {
                WriteObject(ex.Message.ToString());
            }
        }
    }
}
