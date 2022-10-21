
-- Used for the optional customer was home/ not at home data and package integrity
IF EXISTS( SELECT * FROM SYS.TABLES WITH(NOLOCK) WHERE NAME = 'POST_SHIPMENT_DATA' )
	DROP TABLE POST_SHIPMENT_DATA
GO

-- Used for additional notes
IF EXISTS( SELECT * FROM SYS.TABLES WITH(NOLOCK) WHERE NAME = 'SHIPMENT_ADDITIONAL_NOTES' )
	DROP TABLE SHIPMENT_ADDITIONAL_NOTES
GO

-- The previous two tables are separated, because the data is optional and if it is not filled in the application, there is no need for them to be saved.
-- In the other case there will be data redundancy.

-- Used for the detailed data of the shipment( sender name and address, receiver name and address, tracking number, etc. )
IF EXISTS( SELECT * FROM SYS.TABLES WITH(NOLOCK) WHERE NAME = 'SHIPMENT_DETAILED_DATA' )
	DROP TABLE SHIPMENT_DETAILED_DATA
GO

-- The primary data for the shipment - ID, truckID and the shipment status
IF EXISTS( SELECT * FROM SYS.TABLES WITH(NOLOCK) WHERE NAME = 'SHIPMENTS' )
	DROP TABLE SHIPMENTS
GO

CREATE TABLE SHIPMENTS
(
	ID INT NOT NULL IDENTITY(1,1),
	TRUCK_ID INT NOT NULL,
	SHIPMENT_STATUS SMALLINT NOT NULL,
	CONSTRAINT PK_SHIPMENTS_ID PRIMARY KEY (ID)
)
GO

CREATE NONCLUSTERED INDEX IX_SHIPMENTS_SHIPMENT_STATUS
ON SHIPMENTS(SHIPMENT_STATUS)
GO

CREATE NONCLUSTERED INDEX IX_SHIPMENTS_SHIPMENT_TRUCK_ID
ON SHIPMENTS(TRUCK_ID)
GO

CREATE TABLE SHIPMENT_DETAILED_DATA
(
	ID INT NOT NULL IDENTITY(1,1),
	SHIPMENT_ID INT NOT NULL,
	SENDER_NAME NVARCHAR(64) NOT NULL,
	SENDER_ADDRESS NVARCHAR(128) NOT NULL,
	RECEIVER_NAME NVARCHAR(64) NOT NULL,
	RECEIVER_ADDRESS NVARCHAR(128) NOT NULL,
	TRACKING_NUMBER NVARCHAR(64) NOT NULL,
	PACKAGES_COUNT SMALLINT NOT NULL,
	CONSTRAINT PK_SHIPMENT_DETAILED_DATA_ID PRIMARY KEY (ID)
)
GO

-- Foreign Key to SHIPMENTS table
ALTER TABLE SHIPMENT_DETAILED_DATA
ADD CONSTRAINT FK_SHIPMENT_DETAILED_DATA_SHIPMENTS_ID
FOREIGN KEY (SHIPMENT_ID)
REFERENCES SHIPMENTS(ID)
GO

CREATE TABLE POST_SHIPMENT_DATA
(
	ID INT NOT NULL IDENTITY(1,1),
	SHIPMENT_ID INT NOT NULL,
	CUSTOMER_SHIPMENT_ACCEPTANCE_STATUS SMALLINT NOT NULL,
	PACKAGE_INTEGRITY SMALLINT NOT NULL,
	CONSTRAINT PK_POST_SHIPMENT_DATA_ID PRIMARY KEY (ID)
)
GO

-- Foreign Key to SHIPMENTS table
ALTER TABLE POST_SHIPMENT_DATA
ADD CONSTRAINT FK_POST_SHIPMENT_DATA_SHIPMENTS_ID
FOREIGN KEY (SHIPMENT_ID)
REFERENCES SHIPMENTS(ID)
GO

CREATE TABLE SHIPMENT_ADDITIONAL_NOTES
(
	ID INT NOT NULL IDENTITY(1,1),
	SHIPMENT_ID INT NOT NULL,
	ADDITIONAL_NOTES NVARCHAR(128) NOT NULL,
	CONSTRAINT PK_SHIPMENT_ADDITIONAL_NOTES_ID PRIMARY KEY (ID)
)
GO

-- Foreign Key to SHIPMENTS table
ALTER TABLE SHIPMENT_ADDITIONAL_NOTES
ADD CONSTRAINT FK_SHIPMENT_ADDITIONAL_NOTES_SHIPMENTS_ID
FOREIGN KEY (SHIPMENT_ID)
REFERENCES SHIPMENTS(ID)
GO

IF NOT EXISTS( SELECT * FROM SHIPMENTS WITH(NOLOCK) )
BEGIN

	BEGIN TRANSACTION;  
	  
	BEGIN TRY  
	
	    INSERT INTO SHIPMENTS
		VALUES( 1 /*TRUCK_ID*/, 1 /*SHIPMENT_STATUS*/),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 1, 1 ),
		( 2, 1 ),
		( 2, 1 ),
		( 2, 1 ),
		( 2, 1 ),
		( 2, 1 )
	
		INSERT INTO SHIPMENT_DETAILED_DATA
		VALUES( 1, 'Iga Reid'	, '06260 Brooke Branch, Apt. 004, 76729-7312, Anabelfort, Texas, United States'					, 'Aaisha Mcfadden'		, '314 Alva Circles, Suite 120, 11997, Jaydonport, Wisconsin, United States'				,       'e4d6161a-8ba1-4603-a4a8-08be1e1a8c7f', 1),
		( 2, 'Muskego, WI 53150', '514 Hermann Dale, Apt. 848, 87782, Armandobury, Colorado, United States'						, 'Zoey Morrison'		, '0883 Zieme Trail, Apt. 139, 67336, East Ray, Maine, United States'						,       '54e3a723-7778-432c-9e69-441e6a1995da', 2),
		( 3, 'Rihanna Buckley'	, '406 Wuckert Key, Apt. 667, 29794, Ullrichville, Idaho, United States'						, 'Sanjeev Mcarthur'	, '01384 Vandervort Wall, Apt. 842, 73099, Lake Daisha, Minnesota, United States'			,       '3efd756a-0fe6-4699-9934-0520492a6f18', 3),
		( 4, 'Gordon Osborne'	, '02425 Erdman Branch, Suite 850, 99769, Schmittshire, Minnesota, United States'				, 'Anastazja Crowther'	, '6061 Jacobi Loop, Apt. 370, 16120, New Ryley, Maryland, United States'					,       '2010d16a-f021-448c-9303-141673fd4f78', 2),
		( 5, 'Poppie Noel'		, '9316 Cormier Lock, Apt. 802, 40266-1840, Michaelafort, North Carolina, United States'		, 'Hoorain Ray'			, '15623 Armstrong Cape, Suite 650, 34337-1451, North Alexandria, California, United States',		'262c84c7-e9bd-4a37-ac69-ff64b76cf207', 1), 
		( 6, 'Karam Sykes'		, '0880 Angeline Skyway, Suite 405, 75058, Robertsburgh, Vermont, United States'				, 'Harper Dunlop'		, '68844 Drew Fields, Suite 031, 68571-3108, North Alec, Arkansas, United States'			,       '596e2f4f-7300-4778-8205-efdc7bf33511', 2),
		( 7, 'Haydn Barlow'		, '643 Ed Tunnel, Suite 285, 08968-3319, Kutchview, Nevada, United States'						, 'Paulina Townsend'	, '425 Heller Forest, Apt. 025, 14608-8962, Hartmannview, North Dakota, United States'		,       '51926e1e-880f-49cb-8f01-f2d3cfa70ea4', 1),
		( 8, 'Sebastian Crane'	, '032 Buford Landing, Suite 078, 44490-9386, Bayerborough, Utah, United States'				, 'Xanthe Short'		, '12248 Seth Stream, Suite 544, 67273, Thielhaven, Oklahoma, United States'				,       '13865d7f-a571-487d-9004-2d17520547ea', 2),
		( 9, 'Ebrahim Marriott'	, '198 Laverna Pass, Apt. 839, 51928-4563, South Eleanoremouth, Alabama, United States'			, 'Maizie Portillo'		, '32019 Hansen Parks, Suite 951, 95111, Lake Ruthbury, California, United States'			,       'da591c53-6a0d-4e13-b26f-06d7f020a1ea', 2),
		( 10, 'Caleb Beach'		, '492 Belle Drive, Apt. 906, 44719, Port Wavastad, Nevada, United States'						, 'Joyce Mclaughlin'	, '760 Zachariah Flat, Suite 049, 99776, West Rupertport, Colorado, United States'			,       'b3487613-e98d-48b0-8e6d-7c6227570a96', 2),
		( 11, 'Sallie Sanderson'	, '83404 Kilback Points, Apt. 262, 22542, Emieborough, Maryland, United States'					, 'Sheldon Lennon'		, '997 Kuphal Corner, Apt. 813, 91826-2693, Lake Mossie, Louisiana, United States'			,		'1a1e2432-b593-4e02-a735-6bd1cf7f923d', 1),
		( 12, 'Dilan Driscoll'	, '39481 Chaya Plain, Apt. 996, 19697-8829, Kautzermouth, Minnesota, United States'				, 'Aryaan Zavala'		, '4899 Taryn Plaza, Apt. 223, 45022, New Nealchester, New York, United States'				,       '4bdeaecb-54ea-4cdf-80e2-e8df2b8bb9ad', 1),
		( 13, 'Ines Sandoval'	, '178 Izabella Park, Suite 896, 85840, Lake Tamaratown, South Carolina, United States'			, 'Adeeb Silva'			, '71568 O''Connell Pines, Apt. 740, 88706-1491, East Arvel, Texas, United States'			,       'a006b5ae-4eca-4ee2-b6eb-029a1e4125b3', 2),
		( 14, 'Tayah Wharton'	, '56089 Jalon Junction, Apt. 308, 68161, Torphychester, Nevada, United States'					, 'Bernice Mcgregor'	, '8137 Bruen Common, Suite 767, 91976-9931, Lakinhaven, South Dakota, United States'		,       '3bd03213-eaef-448d-80b8-44071c1b8875', 3),
		( 15, 'Coco Hanson'		, '2361 Jayne Forge, Suite 505, 12372-7242, New Winfield, California, United States'			, 'Minahil Hail'		, '508 Hodkiewicz Pike, Suite 250, 48878, Runtechester, Mississippi, United States'			,       '3b048c05-b6f6-4137-a1ab-b0dd377aedee', 2),
		( 16, 'Jamel Villalobos'	, '6125 Rebeka Brook, Apt. 113, 52891-7112, West Izaiahmouth, Nebraska, United States'			, 'Donald Feeney'		, '32051 Heathcote Unions, Apt. 299, 72091-5104, Port Anderson, Michigan, United States'	,		'a6f76714-350b-4214-a698-a8c18baf11cc', 3),
		( 17, 'Campbell Appleton', '15942 Mossie Lights, Suite 518, 75653, Collierstad, Indiana, United States'					, 'Darrel Love'			, '87740 Rylan Canyon, Apt. 497, 80580-2541, West Karinahaven, Rhode Island, United States'	,		'c2fc6313-3a75-46d0-ab4e-60b7462054ab', 2),
		( 18, 'Sienna Underwood'	, '83414 Mose Estates, Suite 541, 37664-3242, East Mortonstad, Wyoming, United States'			, 'Jia Bevan'			, '8221 Rogahn Prairie, Suite 376, 26232, New Vincenza, Alabama, United States'				,       'ec7e37d1-f3a8-4f1d-a710-e2654f47de55', 2),
		( 19, 'Josie Cole'		, '01804 Connor Mission, Suite 030, 99051, New Chelsieview, North Carolina, United States'		, 'Aniya Burris'		, '108 Ida Fork, Apt. 767, 42128, Leannonmouth, Wyoming, United States'						,       'bfed7aa8-b121-4e4e-812e-fe0baf463770', 1),
		( 20, 'Albie Hastings'	, '8072 Lawson Trail, Suite 182, 65884, Verdieside, New York, United States'					, 'Hussain Wood'		, '22022 Toy Mountains, Suite 234, 42552, Tristianbury, Maryland, United States'			,       '7b16f7ea-b31a-4c11-a733-747ef1c14b50', 2),
		( 21, 'Jude Hickman'		, '89365 Kuhlman Mountain, Suite 336, 88198-2423, East Glennamouth, Connecticut, United States'	, 'Jay-Jay Robinson'	, '814 Legros Stravenue, Suite 742, 99521-7539, Codyshire, Nevada, United States'			,       '45fbea01-81fc-423f-9bf0-01c2f7c1b101', 1),
		( 22, 'Mitchel Hills'	, '070 Kunze Club, Suite 113, 25714-6983, New Logan, Ohio, United States'						, 'Muhammed Holland'	, '93311 Runolfsson Motorway, Suite 955, 44448, Arlieshire, Kentucky, United States'		,       '9097c5fb-7b46-420b-a2bb-ef04b72dc7ee', 2),
		( 23, 'Tommy Mccoy'		, '305 Werner Ridge, Suite 772, 92359, South Vivian, Arizona, United States'					, 'Bryn Hamer'			, '24134 Orlo Crossroad, Suite 446, 49833, South Kaitlyn, Kansas, United States'			,		'e0d1b92e-cd0f-4859-a885-e04c4a7cf95e', 4),
		( 24, 'Solomon Mcmillan'	, '79677 Albertha Port, Suite 135, 98890, Spencermouth, Oregon, United States'					, 'Mikolaj Forbes'		, '6407 Lubowitz Club, Suite 410, 19043, Shannaport, Washington, United States'				,		'a66d848f-8e69-4526-8713-1f7bfd9a430f', 2),
		( 25, 'Humairaa Li'		, '45331 Emilio Fall, Apt. 001, 28602-1473, New Verdafort, Kentucky, United States'				, 'Niamh Howard'		, '912 Aurelia Fields, Suite 389, 85564, Lolitastad, Texas, United States'					,       '2a497425-1b2a-4313-8548-6a9c560136a9', 2),
		( 26, 'Chloe Garner'		, '465 Otha Ville, Suite 543, 94944, New Scottie, Wyoming, United States'						, 'Dawn Finley'			, '6415 Spencer Shores, Suite 003, 03489, Meganestad, Mississippi, United States'			,       '7b84fffa-bdd0-4c31-9f09-2e6117dabd11', 1),
		( 27, 'Glen Villa'		, '68454 Bridie Rest, Suite 508, 46876-3194, New Abbeytown, Indiana, United States'				, 'Asiya Finney'		, '5503 Jacklyn Estates, Apt. 130, 23698, Jordaneborough, Mississippi, United States'		,       '12c87b1a-106c-42bf-a87b-6260cac5dabd', 2),
		( 28, 'Gerald Aguirre'	, '66863 Jordane Vista, Apt. 551, 06836-7503, Zoraside, Florida, United States'					, 'Enrico Atkinson'		, '27435 Tremblay Plaza, Suite 843, 27360-8504, Kalliefort, West Virginia, United States'	,		'b2666f0a-2788-4060-a387-3d717a138b68', 2),
		( 29, 'Vijay Krause'		, '58740 Johnson Trafficway, Suite 419, 59487-8843, Schillermouth, Missouri, United States'		, 'Jonty Orr'			, '72723 Lexie Radial, Suite 233, 73230, Crooksburgh, Connecticut, United States'			,       '55e60647-a9d1-42f9-98dc-5d33bb074ac1', 2),
		( 30, 'Merryn Rosas'		, '98360 Breitenberg Ports, Apt. 616, 59827-2805, Carsonville, Florida, United States'			, 'Waseem Mayo'			, '425 Bianka Springs, Apt. 296, 92377-4372, Jettborough, Iowa, United States'				,		'fc52579c-cf96-4014-bdf7-3c135030eae5', 2),
		( 31, 'Nayla Mann'		, '672 Morton Ramp, Apt. 979, 80791-6633, West Aliciaborough, South Carolina, United States'	, 'Tulisa Larsen'		, '0707 Hegmann Village, Apt. 904, 40827, Hardyborough, North Carolina, United States'		,		'b8fa4762-66f0-4337-8593-bffae7a6a45b', 3),
		( 32, 'Sulayman Conroy'	, '7421 Connie Shore, Apt. 607, 29667, South Amelie, Vermont, United States'					, 'Drake Ewing'			, '46683 Keebler Circles, Apt. 967, 22794, Aliceton, North Carolina, United States'			,       '4e613cc5-c361-4f7a-8c15-c97129c1b728', 1),
		( 33, 'Arham Christie'	, '252 Dexter Mill, Apt. 058, 27014, Piperville, Arkansas, United States'						, 'Travis Gould'		, '497 King Key, Apt. 571, 13826-7449, Shieldsmouth, New Mexico, United States'				,       'e753fef2-466d-457f-adaf-862c1253f8a3', 1)
	
	END TRY  
	BEGIN CATCH  
	    SELECT   
	        ERROR_NUMBER() AS ErrorNumber  
	        ,ERROR_SEVERITY() AS ErrorSeverity  
	        ,ERROR_STATE() AS ErrorState  
	        ,ERROR_PROCEDURE() AS ErrorProcedure  
	        ,ERROR_LINE() AS ErrorLine  
	        ,ERROR_MESSAGE() AS ErrorMessage;  
	  
	    IF @@TRANCOUNT > 0  
	        ROLLBACK TRANSACTION;  
	END CATCH;  
	  
	IF @@TRANCOUNT > 0  
	    COMMIT TRANSACTION
END
GO
