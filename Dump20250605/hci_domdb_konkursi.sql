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
-- Table structure for table `konkursi`
--

DROP TABLE IF EXISTS `konkursi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `konkursi` (
  `OglasID` int NOT NULL,
  `SadrzajOglasa` text COLLATE utf8_unicode_ci,
  `Dokument` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`OglasID`),
  CONSTRAINT `konkursi_ibfk_1` FOREIGN KEY (`OglasID`) REFERENCES `oglas` (`OglasID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `konkursi`
--

LOCK TABLES `konkursi` WRITE;
/*!40000 ALTER TABLE `konkursi` DISABLE KEYS */;
INSERT INTO `konkursi` VALUES (24,'Obavjestavaju se studenti stanari studentskog centra Nikola Tesla da se raspisuje konkurs za useljenje u paviljone 1 i 2.\r\nKonkurs traje od 15.Septembar.2025 do 12.Oktobar.2025 godine.\r\nVise informacija u prilozenom dokumentu.','C:\\Users\\Toba\\Desktop\\WPF-UI__AnimatedSlidingLoginAndSignUpForm-1-main\\Resources\\PDFs\\Konkursi\\BP - 2022-09-01.pdf'),(25,'Rezultati konkursa i raspored studenata po paviljonima i sobam se nalazi u prilozenom dokumentu.\r\n\r\ntel: 053/225-108\r\nemail: scnt@gmail.com\r\nadresa: Majke Jugovica 1 Banja Luka','C:\\Users\\Toba\\Desktop\\WPF-UI__AnimatedSlidingLoginAndSignUpForm-1-main\\Resources\\PDFs\\Konkursi\\OI - Usmeni dio ispita - Odgovori (1).pdf'),(26,'Obavjestavaju se studenti da se produzava rok za prijavu.\r\nVise informacija u prilozenom dokumentu.\r\n\r\ntel: 053/226-108\r\nemail: scnt@gmail.com\r\nadresa: Majke Jugovica 1, Banja Luka','C:\\Users\\Toba\\Desktop\\WPF-UI__AnimatedSlidingLoginAndSignUpForm-1-main\\Resources\\PDFs\\Konkursi\\Radenko-Lukić-CV.pdf');
/*!40000 ALTER TABLE `konkursi` ENABLE KEYS */;
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
