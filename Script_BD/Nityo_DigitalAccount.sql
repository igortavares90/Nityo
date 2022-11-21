CREATE DATABASE `nityo` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

CREATE TABLE `digitalaccount` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AccountId` int DEFAULT NULL,
  `AccountBalance` decimal(9,2) NOT NULL,
  `UpdateDate` datetime NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `transactiontype` (
  `id` int NOT NULL AUTO_INCREMENT,
  `description` varchar(45) NOT NULL,
  `creditdebit` varchar(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `digitalaccounttransaction` (
  `id` int NOT NULL AUTO_INCREMENT,
  `digitalaccountid` int NOT NULL,
  `value` decimal(9,2) NOT NULL,
  `balanceaftertransaction` decimal(9,2) NOT NULL,
  `transactiontypeid` int NOT NULL,
  `transactiondate` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

INSERT INTO `nityo`.`digitalaccount`
(`AccountId`,
`AccountBalance`,
`UpdateDate`)
VALUES
(1,0,now());