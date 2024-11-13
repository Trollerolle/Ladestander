--Once we are connected to the server, open the new query editor window. First, to create the database account, execute the following query in the MSDB database: 
Use MSDB
go
IF NOT EXISTS(SELECT * FROM msdb.dbo.sysmail_account WHERE  name = 'ElBookingMail') 
  BEGIN 
    --CREATE Account [SQLServer Express] 
    EXECUTE msdb.dbo.sysmail_add_account_sp 
    @account_name            = 'ElBookingMail', 
    @email_address           = 'El.Booking24@gmail.com', 
    @display_name            = 'ElBookingMail', 
    @replyto_address         = 'El.Booking24@gmail.com', 
    @description             = '', 
    @mailserver_name         = 'smtp.gmail.com', 
    @mailserver_type         = 'SMTP', 
    @port                    = '587', 
    @username                = 'El.Booking24@gmail.com', 
    @password                = 'sijt gsus eqzu zckj',
    @use_default_credentials =  0 , 
    @enable_ssl              =  1 ; 

  END --IF EXISTS  account


-- To assign a database mail account to the database mail profile, execute the following query in the MSDB database: 

Use MSDB
go
IF NOT EXISTS(SELECT * FROM msdb.dbo.sysmail_profile WHERE  name = 'ElBookingMail')  
  BEGIN 
    --CREATE Profile [SQLServer Express Edition] 
    EXECUTE msdb.dbo.sysmail_add_profile_sp 
      @profile_name = 'ElBookingMail', 
      @description  = 'This db mail account is used by SQL Server Express edition.'; 
  END --IF EXISTS profile

  --To assign a database mail account to the database mail profile, execute the following query in the MSDB database: 
Use MSDB
go
IF NOT EXISTS(SELECT * 
              FROM msdb.dbo.sysmail_profileaccount pa 
                INNER JOIN msdb.dbo.sysmail_profile p ON pa.profile_id = p.profile_id 
                INNER JOIN msdb.dbo.sysmail_account a ON pa.account_id = a.account_id   
              WHERE p.name = 'ElBookingMail' 
                AND a.name = 'ElBookingMail')  
  BEGIN 
    -- Associate Account [SQLServer Express] to Profile [SQLServer Express Edition] 
    EXECUTE msdb.dbo.sysmail_add_profileaccount_sp 
      @profile_name = 'ElBookingMail', 
      @account_name = 'ElBookingMail', 
      @sequence_number = 1 ; 
  END

  -- Grant
  EXECUTE msdb.dbo.sysmail_add_principalprofile_sp
@profile_name = 'ElBookingMail',
@principal_name = 'public',
@is_default = 1;


  -- SEND EN TEST MAIL
  EXEC msdb.dbo.sp_send_dbmail  
    @profile_name = 'ElBookingMail',  
    @recipients = 'info@elbooking.dk',  
    @body = 'Voila..!! This email has been sent from SQL Server Express Edition.',  
    @subject = 'Voila..!! This email has been sent from SQL Server Express Edition.' ;

	-- MAIL kø status

	EXEC msdb.dbo.sysmail_help_queue_sp @queue_type = 'Mail';


	--

	IF EXISTS (
    SELECT 1 FROM sys.configurations 
    WHERE NAME = 'Database Mail XPs' AND VALUE = 0)
BEGIN
  PRINT 'Enabling Database Mail XPs'
  EXEC sp_configure 'show advanced options', 1;  
  RECONFIGURE
  EXEC sp_configure 'Database Mail XPs', 1;  
  RECONFIGURE  
END

--
EXECUTE dbo.sysmail_start_sp;