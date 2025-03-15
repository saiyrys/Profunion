-- MySQL dump 10.13  Distrib 8.0.18, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: profunion
-- ------------------------------------------------------
-- Server version	8.0.18

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
-- Table structure for table `application`
--

DROP TABLE IF EXISTS `application`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `application` (
  `id` varchar(255) NOT NULL,
  `eventId` varchar(255) DEFAULT NULL,
  `userId` bigint(20) NOT NULL,
  `places` int(11) NOT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `userId` (`userId`),
  KEY `eventId` (`eventId`),
  CONSTRAINT `application_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`userId`),
  CONSTRAINT `application_ibfk_2` FOREIGN KEY (`eventId`) REFERENCES `events` (`eventId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `application`
--

LOCK TABLES `application` WRITE;
/*!40000 ALTER TABLE `application` DISABLE KEYS */;
/*!40000 ALTER TABLE `application` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `id` varchar(255) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES ('9636dcaf-b931-471d-a97d-32105ce9b9ea','Концерт');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comments` (
  `id` varchar(255) NOT NULL,
  `content` varchar(2250) NOT NULL,
  `userId` bigint(20) NOT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `userId` (`userId`),
  CONSTRAINT `comments_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventcategories`
--

DROP TABLE IF EXISTS `eventcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `eventcategories` (
  `categoriesId` varchar(255) DEFAULT NULL,
  `eventId` varchar(255) DEFAULT NULL,
  KEY `categoriesId` (`categoriesId`),
  KEY `eventId` (`eventId`),
  CONSTRAINT `eventcategories_ibfk_1` FOREIGN KEY (`categoriesId`) REFERENCES `categories` (`id`),
  CONSTRAINT `eventcategories_ibfk_2` FOREIGN KEY (`eventId`) REFERENCES `events` (`eventId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventcategories`
--

LOCK TABLES `eventcategories` WRITE;
/*!40000 ALTER TABLE `eventcategories` DISABLE KEYS */;
INSERT INTO `eventcategories` VALUES ('9636dcaf-b931-471d-a97d-32105ce9b9ea','d0bc092c-ae32-4973-a3c7-8db49fd71c8a');
/*!40000 ALTER TABLE `eventcategories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `events`
--

DROP TABLE IF EXISTS `events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `events` (
  `eventId` varchar(255) NOT NULL,
  `title` varchar(255) NOT NULL,
  `organizer` varchar(255) DEFAULT NULL,
  `description` varchar(500) NOT NULL,
  `date` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `link` varchar(255) DEFAULT NULL,
  `totalPlaces` int(11) DEFAULT NULL,
  `places` int(11) NOT NULL,
  `isActive` tinyint(1) DEFAULT '1',
  `status` tinyint(1) DEFAULT '0',
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`eventId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `events`
--

LOCK TABLES `events` WRITE;
/*!40000 ALTER TABLE `events` DISABLE KEYS */;
INSERT INTO `events` VALUES ('d0bc092c-ae32-4973-a3c7-8db49fd71c8a','test 1','asfasf','asfgsaf','2025-03-15 08:50:00','google.com',10,10,0,0,'2025-03-10 08:50:19','2025-03-10 08:50:19');
/*!40000 ALTER TABLE `events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `eventuploads`
--

DROP TABLE IF EXISTS `eventuploads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `eventuploads` (
  `eventId` varchar(255) DEFAULT NULL,
  `fileId` varchar(255) DEFAULT NULL,
  KEY `fileId` (`fileId`),
  KEY `eventId` (`eventId`),
  CONSTRAINT `eventuploads_ibfk_1` FOREIGN KEY (`fileId`) REFERENCES `uploads` (`id`),
  CONSTRAINT `eventuploads_ibfk_2` FOREIGN KEY (`eventId`) REFERENCES `events` (`eventId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `eventuploads`
--

LOCK TABLES `eventuploads` WRITE;
/*!40000 ALTER TABLE `eventuploads` DISABLE KEYS */;
/*!40000 ALTER TABLE `eventuploads` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `news`
--

DROP TABLE IF EXISTS `news`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `news` (
  `newsId` varchar(255) NOT NULL,
  `title` varchar(255) NOT NULL,
  `description` varchar(255) NOT NULL,
  `content` varchar(2550) NOT NULL,
  `views` int(11) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`newsId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `news`
--

LOCK TABLES `news` WRITE;
/*!40000 ALTER TABLE `news` DISABLE KEYS */;
INSERT INTO `news` VALUES ('f41c8e70-39bb-415d-a46b-6e9a9be38564','НОВОСТЬ 2','string','string',14,'2025-03-08 18:31:10','2025-03-08 18:31:10'),('f7d9b6bc-2600-4bb3-a152-a65ff6d867cd','Новость','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec eget felis leo. Pellentesque arcu augue, consectetur vel urna quis, ','Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec eget felis leo. Pellentesque arcu augue, consectetur vel urna quis, volutpat mattis nunc. Nulla placerat convallis ante, ac pharetra nibh suscipit.',14,'2025-03-08 12:46:19','2025-03-09 17:08:40');
/*!40000 ALTER TABLE `news` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `newsuploads`
--

DROP TABLE IF EXISTS `newsuploads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `newsuploads` (
  `newsId` varchar(255) DEFAULT NULL,
  `fileId` varchar(255) DEFAULT NULL,
  KEY `fileId` (`fileId`),
  KEY `newsId` (`newsId`),
  CONSTRAINT `newsuploads_ibfk_1` FOREIGN KEY (`fileId`) REFERENCES `uploads` (`id`),
  CONSTRAINT `newsuploads_ibfk_2` FOREIGN KEY (`newsId`) REFERENCES `news` (`newsId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `newsuploads`
--

LOCK TABLES `newsuploads` WRITE;
/*!40000 ALTER TABLE `newsuploads` DISABLE KEYS */;
/*!40000 ALTER TABLE `newsuploads` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `report`
--

DROP TABLE IF EXISTS `report`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `report` (
  `id` varchar(255) NOT NULL,
  `content` varchar(2250) NOT NULL,
  `userId` bigint(20) NOT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `userId` (`userId`),
  CONSTRAINT `report_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `report`
--

LOCK TABLES `report` WRITE;
/*!40000 ALTER TABLE `report` DISABLE KEYS */;
/*!40000 ALTER TABLE `report` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `uploads`
--

DROP TABLE IF EXISTS `uploads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `uploads` (
  `id` varchar(255) NOT NULL,
  `fileName` varchar(255) DEFAULT NULL,
  `filePath` varchar(255) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uploads`
--

LOCK TABLES `uploads` WRITE;
/*!40000 ALTER TABLE `uploads` DISABLE KEYS */;
INSERT INTO `uploads` VALUES ('0d262a4b-9a8b-40af-b706-5139485a550c','97d6d4f6-b198-4501-a62a-8961aa42972a.jpg','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\97d6d4f6-b198-4501-a62a-8961aa42972a.jpg','2025-03-10 10:23:39',NULL),('3edfcc7b-409e-443e-9122-2b631f772f68','ce332701-2e61-4cf6-a225-125b76075a33.jpg','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\ce332701-2e61-4cf6-a225-125b76075a33.jpg','2025-03-10 10:50:46',NULL),('45313fc1-6ec5-4c96-bee2-45179bfa2ab0','253a6e77-fc68-4e4e-b546-b6fe94f5e008.png','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\253a6e77-fc68-4e4e-b546-b6fe94f5e008.png','2025-03-10 10:50:15',NULL),('628178d9-5c17-4664-9d21-37fbb3914c23','72347fe7-04b7-4b3a-b612-4bbf2bd59edc.jpg','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\72347fe7-04b7-4b3a-b612-4bbf2bd59edc.jpg','2025-03-10 10:50:09',NULL),('62998947-020d-4c92-aff9-ebf295435d4d','f66cbf02-5349-425d-9b01-2d7bb53e6ca3.png','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\f66cbf02-5349-425d-9b01-2d7bb53e6ca3.png','2025-03-10 10:46:55',NULL),('72ceb7a0-0271-4642-8b62-d394774d491c','03008e3b-27f6-427f-9a00-cd085c16bd6b.png','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\03008e3b-27f6-427f-9a00-cd085c16bd6b.png','2025-03-10 10:24:07',NULL),('a1a662b2-fb72-4765-abd4-535f5eecdaee','093a1564-b8ee-4323-8581-e293f1eeefa2.png','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\093a1564-b8ee-4323-8581-e293f1eeefa2.png','2025-03-10 10:23:36',NULL),('b10ec6be-677d-490e-8bc4-60387b9e7ee6','0d5c421d-ce0e-488a-8c79-22769f3854f5.png','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\0d5c421d-ce0e-488a-8c79-22769f3854f5.png','2025-03-10 10:22:47',NULL),('c3bcaed5-e795-4823-8c99-a2033011af80','2d2f0c6b-b693-4507-9f24-f58fb3ced2bb.jpg','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\2d2f0c6b-b693-4507-9f24-f58fb3ced2bb.jpg','2025-03-10 10:23:50',NULL),('fa31a7a5-0df2-438b-9897-a0cb31da075a','1d4d11be-0337-4255-8175-8d55799208ee.jfif','C:\\Users\\Павел\\Desktop\\Profunions\\src\\profunion.API\\bin\\Debug\\net6.0\\uploads\\uploads\\EventUploads\\1d4d11be-0337-4255-8175-8d55799208ee.jfif','2025-03-10 10:46:50',NULL);
/*!40000 ALTER TABLE `uploads` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `userId` bigint(20) NOT NULL,
  `userName` varchar(255) NOT NULL,
  `firstName` varchar(255) NOT NULL,
  `middleName` varchar(255) NOT NULL,
  `lastName` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `salt` varchar(255) NOT NULL,
  `role` enum('USER','ADMIN','MODER') NOT NULL DEFAULT (_utf8mb4'USER'),
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`userId`),
  UNIQUE KEY `userName` (`userName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (-6421685180565551745,'pavel','pavel','slex','rep','alex@mail.ru','Zfcs8/M+FhKTITSg5udGzBdiweAqhPK57Ho5OaMKznI=','WxbuPUWPLbxsBEtc+fagY/kNOk18kM1k8o01ktw5xlA1bG84tignBeNual7Cr4CnoBgbxDHeK12+tJhCefLe1Q==','ADMIN','2025-03-09 18:14:43','2025-03-09 15:17:02'),(3390108875705823123,'admin','admin','admin','АДМИНИСТРАТОР','string@safasf.com','Y5xRZoQvNy99Hcyk5Bd8iSgSnE9goNePrsS9F2uAw3Y=','wQlPqBk/Kp3o1NAysQOKRRFKwQkKrpzpPyuGmVIG8252hIfsfe3zxc3RmLCkE2QFYi0FGSOu0QK7cVaMs3x1Tg==','ADMIN','2025-03-06 13:41:58','2025-03-08 13:39:56'),(5215063255060740080,'user','ГРУППЫ','СОБ','СТУДЕНТ','string@afasf.com','5yaduJiqgwSDwzMQkxUs/WXf3vWN12OJbGzeW0DthKc=','2O3rE5sFCg9ecO2SlC2BlsxYuqJuFCCEWirlRvYuufZ1w6KmZRXDcHvadDHZQDobNJRyv/NIuX5nMKbtCfPoiw==','MODER','2025-03-06 13:21:16','2025-03-08 13:39:41'),(6132594588260991326,'EFEM','EFEM','','POLITIS','string@mail.ru','yushRvhrUxRfLZ6inBFyTSF7t+kOvBG6KBnphgct+4c=','V1IhO94fe/dMdXG8PC6dZg0jx9Yg4azQFl8D21UJuRIUbdzz12FaMpL57ofXl0n+93E8b8vYm4bNxnJ4y45JUA==','ADMIN','2025-03-06 18:18:21','2025-03-06 16:58:04');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `useruploads`
--

DROP TABLE IF EXISTS `useruploads`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `useruploads` (
  `userId` bigint(20) DEFAULT NULL,
  `fileId` varchar(255) DEFAULT NULL,
  KEY `userId` (`userId`),
  KEY `fileId` (`fileId`),
  CONSTRAINT `useruploads_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`userId`),
  CONSTRAINT `useruploads_ibfk_2` FOREIGN KEY (`fileId`) REFERENCES `uploads` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `useruploads`
--

LOCK TABLES `useruploads` WRITE;
/*!40000 ALTER TABLE `useruploads` DISABLE KEYS */;
/*!40000 ALTER TABLE `useruploads` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-03-10 14:01:58
