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
-- Table structure for table `aktuelnosti`
--

DROP TABLE IF EXISTS `aktuelnosti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aktuelnosti` (
  `OglasID` int NOT NULL,
  `Dokument` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`OglasID`),
  CONSTRAINT `aktuelnosti_ibfk_1` FOREIGN KEY (`OglasID`) REFERENCES `oglas` (`OglasID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aktuelnosti`
--

LOCK TABLES `aktuelnosti` WRITE;
/*!40000 ALTER TABLE `aktuelnosti` DISABLE KEYS */;
INSERT INTO `aktuelnosti` VALUES (17,'C:\\Users\\Toba\\Desktop\\WPF-UI__AnimatedSlidingLoginAndSignUpForm-1-main\\Resources\\PDFs\\Aktuelnosti\\Strucna praksa SI - 20250206.pdf'),(52,'C:\\Users\\Toba\\Desktop\\WPF-UI__AnimatedSlidingLoginAndSignUpForm-1-main\\Resources\\PDFs\\Aktuelnosti\\aprilski_20_21.pdf');
/*!40000 ALTER TABLE `aktuelnosti` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-05 11:10:24
