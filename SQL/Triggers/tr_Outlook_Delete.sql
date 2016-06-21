/*!
* DisplayMonkey source file
* http://displaymonkey.org
*
* Copyright (c) 2015 Fuel9 LLC and contributors
*
* Released under the MIT license:
* http://opensource.org/licenses/MIT
*/

use [DisplayMonkey]
GO
if object_id('tr_Outlook_Delete','TR') is null exec('
create trigger dbo.tr_Outlook_Delete
on Outlook after delete as select 1
');
go
/*******************************************************************
  2014-11-28 [LTL] - DisplayMonkey object
*******************************************************************/
alter trigger dbo.tr_Outlook_Delete 
	on Outlook 
	after delete
as
begin
	delete f from Frame f inner join deleted d on d.FrameId=f.FrameId
end
GO
