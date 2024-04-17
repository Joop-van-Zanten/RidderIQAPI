using Ridder.Client.SDK;
using Ridder.Client.SDK.SDKPermissions;
using RidderIQAPI.Models.RidderIQ;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;

namespace RidderIQAPI.Api
{
	/// <summary>
	/// Ridder IQ API handler
	/// </summary>
	internal static partial class ApiRidderIQ
	{
		/// <summary>
		/// Ridder IQ API Permissions handler
		/// </summary>
		internal static class Permissions
		{
			/// <summary>
			/// RidderIQ Permission check: Action(s)
			/// </summary>
			/// <param name="cookies">User cookies for authentication</param>
			/// <param name="checkPermissions">Check permission(s)</param>
			/// <returns></returns>
			public static IEnumerable<RidderIQPermissionActionResult> CheckPermissionActions(
				Collection<CookieHeaderValue> cookies,
				params RidderIQPermissionActionReqeust[] checkPermissions
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permission
				return checkPermissions.Select(x =>
					new RidderIQPermissionActionResult(
						x,
						sdk.CheckPermission(new SDKActionPermissionContext(x.ActionName, (SDKDesignerScope)x.Scope))
					)
				);
			}

			/// <summary>
			/// RidderIQ Permission check: Column(s)
			/// </summary>
			/// <param name="cookies">User cookies for authentication</param>
			/// <param name="checkPermissions">Check permission(s)</param>
			/// <returns></returns>
			public static IEnumerable<RidderIQPermissionColumnResult> CheckPermissionsColumns(
				Collection<CookieHeaderValue> cookies,
				params RidderIQPermissionColumnRequest[] checkPermissions
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permission
				return checkPermissions.Select(x =>
					new RidderIQPermissionColumnResult(
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
			public static IEnumerable<RidderIQPermissionWorkflowResult> CheckPermissionsWorkflows(
				Collection<CookieHeaderValue> cookies,
				params RidderIQPermissionWorkflowReqeust[] checkPermissions
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permission
				return checkPermissions.Select(x =>
					new RidderIQPermissionWorkflowResult(
						x,
						sdk.CheckPermission(new SDKWorkflowPermissionContext(x.WorkflowEvent, (SDKDesignerScope)x.Scope))
					)
				);
			}

			/// <summary>
			/// RidderIQ Permission check: Table(s)
			/// </summary>
			/// <param name="cookies">User cookies for authentication</param>
			/// <param name="checks">Check permission(s)</param>
			/// <returns></returns>
			public static IEnumerable<RidderIQPermissionTableResult> CheckPermissionsTable(
				Collection<CookieHeaderValue> cookies,
				params string[] checks
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permissions
				return checks.Select(x =>
					new RidderIQPermissionTableResult(
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
			public static IEnumerable<RidderIQPermissionCustomResult> CheckPermissionCustomPermission(
				Collection<CookieHeaderValue> cookies,
				params Guid[] checks
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permission
				return checks.Select(x =>
					new RidderIQPermissionCustomResult(
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
			public static IEnumerable<RidderIQPermissionFormPartResult> CheckPermissionFormPart(
				Collection<CookieHeaderValue> cookies,
				params RidderIQPermissionFormPartReqeust[] checks
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permissions
				return checks.Select(x =>
					new RidderIQPermissionFormPartResult(
						x,
						sdk.CheckPermission(new SDKFormPartPermissionContext(x.FormpartName, (SDKDesignerScope)x.Scope))
					)
				);
			}

			public static IEnumerable<RidderIQPermissionScriptResult> CheckPermissionScript(
				Collection<CookieHeaderValue> cookies,
				params RidderIQPermissionScriptReqeust[] checks
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permissions
				return checks.Select(x =>
					new RidderIQPermissionScriptResult(
						x,
						sdk.CheckPermission(new SDKScriptPermissionContext(x.Name, (SDKDesignerScope)x.Scope))
					)
				);
			}

			public static IEnumerable<RidderIQPermissionReportResult> CheckPermissionReport(
				Collection<CookieHeaderValue> cookies,
				params RidderIQPermissionReportReqeust[] checks
			)
			{
				// Get the SDK Client
				RidderIQSDK sdk = Core.GetClient(cookies, true);
				// Check permissions
				return checks.Select(x =>
					new RidderIQPermissionReportResult(
						x,
						sdk.CheckPermission(new SDKReportPermissionContext(x.ReportID, (SDKDesignerScope)x.Scope))
					)
				);
			}
		}
	}
}