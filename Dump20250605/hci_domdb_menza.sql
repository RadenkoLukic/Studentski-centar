-- MySQL dump 10.13  Distrib 8.0.26, for Win64 (x86_64)
--
-- Host: localhost    Database: hci_domdb
-- ------------------------------------------------------
-- Server version	8.0.26

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `menza`
--

DROP TABLE IF EXISTS `menza`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `menza` (
  `MenzaID` int NOT NULL AUTO_INCREMENT,
  `Tekst` text COLLATE utf8_unicode_ci NOT NULL,
  `CijenaDorucka` decimal(10,2) NOT NULL,
  `CijenaRucka` decimal(10,2) NOT NULL,
  `CijenaVecere` decimal(10,2) NOT NULL,
  `TerminDorucka` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `TerminRucka` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `TerminVecere` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `DatumObjave` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `OsobaID` int NOT NULL,
  PRIMARY KEY (`MenzaID`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb3 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `menza`
--

LOCK TABLES `menza` WRITE;
/*!40000 ALTER TABLE `menza` DISABLE KEYS */;
INSERT INTO `menza` VALUES (2,'Cijene i termini za studente koji nisu stanovnici studentskog centra',2.20,4.80,3.00,'07:00 - 09:00','11:30 - 14:30','17:30 - 19:30','2025-02-13 10:31:29',12),(14,'Ishrana u JU Studentski centar „Nikola Tesla“ organizovana je u dva linijska restorana koji se nalaze na dvije različite lokacije. Oni čine studentsku menzu. Menza Paviljona 1 i 2 se nalazi u ulici: Majke Jugovića 1, a kapacitet ovog distributivnog restorana je 230 sjedećih mjesta. Studentska menza Paviljona 3 i 4 se nalazi u ulici Bulevar vojvode Petra Bojovića 1a, a kapacitet ovog distributivnog restorana je 120 sjedećih mjesta.',0.60,1.40,1.00,'07:00 - 09:00','11:30 - 14:30','17:30  - 19:30','2025-06-05 09:16:13',12);
/*!40000 ALTER TABLE `menza` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-05 11:10:23
