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
-- Table structure for table `oglas`
--

DROP TABLE IF EXISTS `oglas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `oglas` (
  `OglasID` int NOT NULL AUTO_INCREMENT,
  `Naslov` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `DatumObjave` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `OsobaID` int NOT NULL,
  PRIMARY KEY (`OglasID`),
  KEY `OsobaID` (`OsobaID`),
  CONSTRAINT `oglas_ibfk_1` FOREIGN KEY (`OsobaID`) REFERENCES `zaposleni` (`OsobaID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb3 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `oglas`
--

LOCK TABLES `oglas` WRITE;
/*!40000 ALTER TABLE `oglas` DISABLE KEYS */;
INSERT INTO `oglas` VALUES (17,'Prvi oglas\r\nObavjestavaju se studenti stanari doma Studentskog centra Nikola Tesla, paviljoni 1 i 2 da ce doci do zatvaranja obe citaonice u trajanju od 7 dana','2025-02-11 22:48:10',12),(24,'Konkurs za septembar','2025-02-12 10:46:37',12),(25,'Konkurs 2','2025-02-12 10:49:51',12),(26,'Konkurs 3','2025-02-12 11:07:13',12),(52,'U periodu od 08-12:00 sati izvoditi će se radovi na hidrantskoj mreži te će biti prekid u snabdijevanju vodom za objekte u P-1 i P-2 u sklopu JU SC \"Nikola Tesla\" Banja Luka.','2025-06-05 09:04:01',12),(53,'Ostale informacije','2025-06-05 09:11:54',12),(54,'Smještaj studenata','2025-06-05 09:13:53',12);
/*!40000 ALTER TABLE `oglas` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-05 11:10:21
