// Copyright (c) 2012-2020 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

using Dicom.Network;
using System;
using System.Collections.Generic;
using System.Threading;
using WorklistSCP.Models;

namespace WorklistSCP
{
   public class WorklistServer
   {
      private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

      private static IDicomServer _server;
      private static Timer _itemsLoaderTimer;


      protected WorklistServer()
      {
      }

      public static string AETitle { get; set; }


      public static IWorklistItemsSource CreateItemsSourceService => new WorklistItemsProvider();

      public static List<WorklistItem> CurrentWorklistItems { get; private set; }

      public static void Start(int port, string aet)
      {
         AETitle = aet;
         Logger.Info($"Start Worklist Server on port:{port} ae:{aet}");
         _server = DicomServer.Create<WorklistService>(port);

         // every 30 seconds the worklist source is queried and the current list of items is cached in _currentWorklistItems
         _itemsLoaderTimer = new Timer((state) =>
         {
            var newWorklistItems = CreateItemsSourceService.GetAllCurrentWorklistItems();
            CurrentWorklistItems = newWorklistItems;
         }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
      }


      public static void Stop()
      {
         Logger.Info("Stop Worklist Server");
         _itemsLoaderTimer?.Dispose();
         _server.Dispose();
      }


   }
}
