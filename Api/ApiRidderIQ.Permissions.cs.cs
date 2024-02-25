using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKPermissions;
using RidderIQAPI.Models.RidderPermission;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;

namespace RidderIQAPI.Api
{
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Ridder IQ Permission check: Action(s)
		/// </summary>
		/// <param name="cookies">User cookies for authentication</param>
		/// <param name="checkPermissions">Check permission(s)</param>
		/// <returns></returns>
		public static IEnumerable<ActionResult> CheckPermissionActions(
			Collection<CookieHeaderValue> cookies,
			params ActionReqeust[] checkPermissions
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permission
			return checkPermissions.Select(x =>
				new ActionResult(
					x,
					sdk.CheckPermission(new SDKActionPermissionContext(x.ActionName, (SDKDesignerScope)x.Scope))
				)
			);
		}

		/// <summary>
		/// Ridder IQ Permission check: Column(s)
		/// </summary>
		/// <param name="cookies">User cookies for authentication</param>
		/// <param name="checkPermissions">Check permission(s)</param>
		/// <returns></returns>
		public static IEnumerable<ColumnResult> CheckPermissionsColumns(
			Collection<CookieHeaderValue> cookies,
			params ColumnRequest[] checkPermissions
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permission
			return checkPermissions.Select(x =>
				new ColumnResult(
					x,
					sdk.CheckPermission(new SDKColumnPermissionContext(x.Table, x.Column, SDKColumnPermissionType.Read)),
					sdk.CheckPermission(new SDKColumnPermissionContext(x.Table, x.Column, SDKColumnPermissionType.Write))
				)
			).ToList();
		}

		/// <summary>
		/// Ridder IQ Permission check: Workflow(s)
		/// </summary>
		/// <param name="cookies">User cookies for authentication</param>
		/// <param name="checkPermissions">Check permission(s)</param>
		/// <returns></returns>
		public static IEnumerable<WorkflowResult> CheckPermissionsWorkflows(
			Collection<CookieHeaderValue> cookies,
			params WorkflowReqeust[] checkPermissions
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permission
			return checkPermissions.Select(x =>
				new WorkflowResult(
					x,
					sdk.CheckPermission(new SDKWorkflowPermissionContext(x.WorkflowEvent, (SDKDesignerScope)x.Scope))
				)
			);
		}

		/// <summary>
		/// Ridder IQ Permission check: Table(s)
		/// </summary>
		/// <param name="cookies">User cookies for authentication</param>
		/// <param name="checks">Check permission(s)</param>
		/// <returns></returns>
		public static IEnumerable<TableResult> CheckPermissionsTable(
			Collection<CookieHeaderValue> cookies,
			params string[] checks
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permissions
			return checks.Select(x =>
				new TableResult(
					x,
					sdk.CheckPermission(new SDKTablePermissionContext(x, SDKTablePermissionType.Insert)),
					sdk.CheckPermission(new SDKTablePermissionContext(x, SDKTablePermissionType.Read)),
					sdk.CheckPermission(new SDKTablePermissionContext(x, SDKTablePermissionType.Write)),
					sdk.CheckPermission(new SDKTablePermissionContext(x, SDKTablePermissionType.Delete))
				)
			).ToList();
		}

		/// <summary>
		/// Ridder IQ Permission check: Custom permission(s)
		/// </summary>
		/// <param name="cookies">User cookies for authentication</param>
		/// <param name="checks">Check permission(s)</param>
		/// <returns></returns>
		public static IEnumerable<CustomResult> CheckPermissionCustomPermission(
			Collection<CookieHeaderValue> cookies,
			params Guid[] checks
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permission
			return checks.Select(x =>
				new CustomResult(
					x,
					sdk.CheckPermission(new SDKCustomPermissionContext(x))
				)
			);
		}

		/// <summary>
		/// Ridder IQ Permission check: Formpart(s)
		/// </summary>
		/// <param name="cookies">User cookies for authentication</param>
		/// <param name="checks">Check permission(s)</param>
		/// <returns></returns>
		public static IEnumerable<FormPartResult> CheckPermissionFormPart(
			Collection<CookieHeaderValue> cookies,
			params FormPartReqeust[] checks
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permissions
			return checks.Select(x =>
				new FormPartResult(
					x,
					sdk.CheckPermission(new SDKFormPartPermissionContext(x.FormpartName, (SDKDesignerScope)x.Scope))
				)
			);
		}

		public static IEnumerable<ScriptResult> CheckPermissionScript(
			Collection<CookieHeaderValue> cookies,
			params ScriptReqeust[] checks
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permissions
			return checks.Select(x =>
				new ScriptResult(
					x,
					sdk.CheckPermission(new SDKScriptPermissionContext(x.Name, (SDKDesignerScope)x.Scope))
				)
			);
		}

		public static IEnumerable<ReportResult> CheckPermissionReport(
			Collection<CookieHeaderValue> cookies,
			params ReportReqeust[] checks
		)
		{
			// Get the SDK Client
			RidderIQSDK sdk = GetClient(cookies, true);
			// Check permissions
			return checks.Select(x =>
				new ReportResult(
					x,
					sdk.CheckPermission(new SDKReportPermissionContext(x.ReportID, (SDKDesignerScope)x.Scope))
				)
			);
		}
	}
}