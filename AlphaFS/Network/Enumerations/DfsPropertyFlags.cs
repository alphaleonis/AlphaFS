/*  Copyright (C) 2008-2016 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace Alphaleonis.Win32.Network
{
   /// <summary>A set of bit flags that describe specific properties of a DFS namespace, root, or link.</summary>
   [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
   [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dfs")]
   [Flags]
   public enum DfsPropertyFlags
   {
      /// <summary>No property flag.</summary>
      None = 0,

      /// <summary>DFS_PROPERTY_FLAG_INSITE_REFERRALS
      /// <para>
      ///   Scope: Domain roots, stand-alone roots, and links.
      ///   If this flag is set at the DFS root, it applies to all links; otherwise, the value of this flag is considered for each individual link.
      /// </para>
      /// <para>
      ///   When this flag is set, a DFS referral response from a DFS server for a DFS root or link with the "INSITE" option enabled contains only
      ///   those targets which are in the same site as the DFS client requesting the referral.
      ///   Targets in the two global priority classes are always returned, regardless of their site location.
      /// </para>
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Insite")]
      InsiteReferrals = 1,

      /// <summary>DFS_PROPERTY_FLAG_ROOT_SCALABILITY
      /// <para>
      ///   Scope: The entire DFS namespace for a domain-based DFS namespace only.
      /// </para>
      /// <para>
      ///   By default, a DFS root target server polls the PDS to detect changes to the DFS metadata.
      ///   To prevent heavy server load on the PDC, root scalability can be enabled for the DFS namespace.
      ///   Setting this flag will cause the DFS server to poll the nearest domain controller instead of the PDC for DFS metadata changes for the common namespace.
      ///   Note that any changes made to the metadata must still occur on the PDC, however.
      /// </para>
      /// </summary>
      RootScalability = 2,

      /// <summary>DFS_PROPERTY_FLAG_SITE_COSTING
      /// <para>
      ///   Scope: The entire DFS namespace for both domain-based and stand-alone DFS namespaces.
      /// </para>
      /// <para>
      ///   By default, targets returned in a referral response from a DFS server to a DFS client for a DFS root or link
      ///   consists of two groups: targets in the same site as the client, and targets outside the site.
      /// </para>
      /// <para>
      ///   If site-costing is enabled for the Active Directory, the response can have more than two groups,
      ///   with each group containing targets with the same site cost for the specific DFS client requesting the referral.
      ///   The groups are ordered by increasing site cost. For more information about how site-costing is used to prioritize targets.
      /// </para>
      /// </summary>
      SiteCosting = 4,

      /// <summary>DFS_PROPERTY_FLAG_TARGET_FAILBACK
      /// <para>
      ///   Scope: Domain-based DFS roots, stand-alone DFS roots, and DFS links.
      ///   If this flag is set at the DFS root, it applies to all links; otherwise, the value of this flag is considered for each individual link.
      /// </para>
      /// <para>
      ///   When this flag is set, optimal target failback is enabled for V4 DFS clients,
      ///   allowing them to fail back to an optimal target after failing over to a non-optimal one.
      ///   The target failback setting is provided to the DFS client in a V4 referral response by a DFS server.
      /// </para>
      /// </summary>
      TargetFailback = 8,

      /// <summary>DFS_PROPERTY_FLAG_CLUSTER_ENABLED
      /// <para>Scope: Stand-alone DFS roots and links only.</para>
      /// <para>The DFS root is clustered to provide high availability for storage failover.</para>
      /// </summary>
      ClusterEnabled = 16,

      /// <summary>DFS_PROPERTY_FLAG_ABDE
      /// <para>Scope: Domain-based DFS roots and stand-alone DFS roots.</para>
      /// <para>When this flag is set, Access-Based Directory Enumeration (ABDE) mode support is enabled on the entire DFS root target share of the DFS namespace.</para>
      /// </summary>
      AccessBasedDirectoryEnumeration = 32
   }
}
