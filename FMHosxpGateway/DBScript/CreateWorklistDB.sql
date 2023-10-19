CREATE TABLE IF NOT EXISTS `tb_xraymwlitems` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccessionNumber` varchar(20) COLLATE tis620_thai_ci NOT NULL,
  `PatientID` varchar(20) COLLATE tis620_thai_ci NOT NULL,
  `Sex` varchar(20) COLLATE tis620_thai_ci DEFAULT NULL,
  `PatientName` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `DateOfBirth` datetime DEFAULT NULL,
  `ReferringPhysician` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `PerformingPhysician` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `Modality` varchar(50) COLLATE tis620_thai_ci DEFAULT NULL,
  `ExamDateAndTime` datetime DEFAULT NULL,
  `ExamRoom` varchar(50) COLLATE tis620_thai_ci DEFAULT NULL,
  `ExamDescription` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `StudyUID` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `ProcedureID` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `ProcedureStepID` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `HospitalName` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `ScheduledAET` varchar(255) COLLATE tis620_thai_ci DEFAULT NULL,
  `IsComplete` char(1) COLLATE tis620_thai_ci NOT NULL DEFAULT 'N',
  `vn` varchar(30) COLLATE tis620_thai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id_UNIQUE` (`Id`),
  UNIQUE KEY `AccessionNumber_UNIQUE` (`AccessionNumber`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=tis620 COLLATE=tis620_thai_ci;

DROP EVENT IF EXISTS `update_worklist`;
DELIMITER //
CREATE EVENT `update_worklist` ON SCHEDULE EVERY 1 DAY STARTS '2020-08-19 00:05:00' ON COMPLETION PRESERVE ENABLE DO UPDATE tb_xraymwlitems set tb_xraymwlitems.IsComplete = 'Y'//
DELIMITER ;