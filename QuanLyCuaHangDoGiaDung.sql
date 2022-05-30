CREATE DATABASE QuanLyCuaHangDoGiaDung
go


go
USE QuanLyCuaHangDoGiaDung

go
CREATE TABLE KhachHang (
	ID_KH varchar(30) not null PRIMARY KEY ,
	Ten_KH nvarchar(30),
	DT_KH nvarchar(30),
	DC_KH nvarchar(50),
	GioiTinh nvarchar(5) CHECK (GioiTinh=N'Nam' or GioiTinh=N'Nữ'))
;
go

go
insert into KhachHang
values (1,N'Nguyễn Văn A','0122234562',N'Hà Nội',N'Nam'),
		(2,N'Nguyễn Văn B','0122234562',N'Hà Nội',N'Nam'),
		(3,N'Nguyễn Thị C','0122234562',N'Hà Nội',N'Nữ'),
		(4,N'Đào Hưng D','0122234562',N'Hà Nội',N'Nữ'),
		(5,N'Bùi Văn E','0122234562',N'Hà Nội',N'Nam');
go

go
CREATE TABLE NCC (
	ID_NCC varchar(30) not null PRIMARY KEY ,
	Ten_NCC nvarchar(30),
	DC_NCC nvarchar(50),
	DT_NCC nvarchar(30),
	TT_GD nvarchar(30) default N'Còn giao dịch')
;
go

go
insert into NCC (ID_NCC , Ten_NCC , DC_NCC , DT_NCC )
values (1,N'Hòa Phát',N'Hà Nội','0122234562'),
		(2,N'Hưng Thịnh',N'Hà Nội','0122234562'),
		(3,N'Vương Mộc',N'Hà Nội','0122234562'),
		(4,N'Hải Anh',N'Hà Nội','0122234562'),
		(5,N'Thịnh Vượng',N'Hà Nội','0122234562');
go

go
CREATE TABLE SanPham (
	ID_SP varchar(30) not null PRIMARY KEY ,
	ID_NCC varchar(30) not null  ,
	Ten_SP nvarchar(50) ,
	DGN_SP float , --Đơn giá nhập SP--
	SL_SP int , --Số lượng SP--
	DonGiaBan float, --Đơn giá bán--
	MT_SP nvarchar (100) , --Mô tả SP--
	TT_SP nvarchar(100) default N'Còn hàng' ,
	FOREIGN KEY (ID_NCC) REFERENCES NCC(ID_NCC) ON DELETE CASCADE ON UPDATE CASCADE )
;
go

go
insert into SanPham (ID_SP,ID_NCC,Ten_SP,DGN_SP,SL_SP,DonGiaBan,MT_SP)
values (1,1,N'Dao Gọt hoa quả','100000','100','150000',N'Dao inox không gỉ'),
		(2,1,N'Kéo lớn','90000','110','130000',N'Lớn nhưng nhỏ gọn'),
		(3,4,N'Chảo Chống Dính','180000','85','250000',N'Chống dính real'),
		(4,2,N'Bàn ăn gia đình','900000','20','1200000',N'Ngồi được 6-10 người'),
		(5,2,N'Ghế đệm','70000','120','129000',N'Siêu êm không nóng'),
		(6,4,N'Nồi inox','85000','250','139000',N'inox!'),
		(7,3,N'Bộ 10 bát đũa','55000','150','99000',N'có đúng 10 bát'),
		(8,5,N'Nồi Cơm','195000','120','299000',N'Nấu được cơm'),
		(9,5,N'Bếp Từ','450000','90','590000',N'Tiết kiệm điện');
go


go
CREATE TABLE HoaDon (
	ID_HD varchar(30) not null PRIMARY KEY ,
	ID_KH varchar(30) not null ,
	Ngay_HD datetime ,
	TT_HD float , --Tổng tiền HD--
	FOREIGN KEY (ID_KH) REFERENCES KhachHang(ID_KH) ON DELETE CASCADE ON UPDATE CASCADE )
;
go

go
insert into HoaDon ( ID_HD , ID_KH , Ngay_HD)
values (1,1,'2021-1-1'),
	(2,2,'2021-5-13'),
	(3,3,'2021-7-2'),
	(4,4,'2021-8-5'),
	(5,5,'2021-9-11');
go

go
CREATE TABLE CTHD (
	ID_HD varchar(30) not null ,
	ID_SP varchar(30) not null  ,
	SL_CTHD int CHECK (SL_CTHD >= 1) ,
	TT_CTHD float ,
	FOREIGN KEY (ID_HD) REFERENCES HoaDon(ID_HD) ON DELETE CASCADE ON UPDATE CASCADE ,
	FOREIGN KEY (ID_SP) REFERENCES SanPham(ID_SP) ON DELETE CASCADE ON UPDATE CASCADE )
;
go

Alter table CTHD
ADD CONSTRAINT PK_CTHD PRIMARY KEY (ID_HD,ID_SP);

go 
insert into CTHD (ID_HD , ID_SP , SL_CTHD)
values (1,1,10),
	(1,5,1),
	(2,6,1),
	(2,2,1),
	(3,4,3),
	(3,3,3),
	(4,1,1),
	(4,5,1),
	(5,7,2);
go

create table DangNhap (
Tai_Khoan varchar(20) not null PRIMARY KEY ,
Mat_Khau varchar (20) not null ,
type int 
)
insert into DangNhap values ('tu','123',1)
insert into DangNhap values ('nam','123',0)

go

--procedure đăng nhập hệ thống chống sql injection--
create procedure sp_LoginA
@user nvarchar(30) , @pass nvarchar(30)
as
begin
	select * from DangNhap where Tai_Khoan = @user and Mat_Khau = @pass and type = 1 
end
execute sp_LoginA tu , 123 
drop procedure sp_LoginA

create procedure sp_LoginB
@user nvarchar(30) , @pass nvarchar(30)
as
begin
	select * from DangNhap where Tai_Khoan = @user and Mat_Khau = @pass and type = 0 
end
execute sp_LoginB nam , 123 
drop procedure sp_LoginB
--THỦ TỤC 2
--procedure tìm kiếm hóa đơn theo tên khách--
CREATE PROCedure sp_TKHD2 @TenK nvarchar(50)
as 
begin
	select HoaDon.ID_HD as [Mã hóa đơn] , HoaDon.ID_KH as [Mã khách] , KhachHang.Ten_KH as [Tên khách] , SanPham.Ten_SP as [Tên sản phẩm] , CTHD.SL_CTHD as [Số lượng mua], HoaDon.Ngay_HD as [Ngày mua hàng], CTHD.TT_CTHD as [Thành tiền], HoaDon.TT_HD as [Tổng tiền]
	from HoaDon , KhachHang , SanPham , CTHD 
	WHERE KhachHang.Ten_KH like '%'+@TenK+'%' and HoaDon.ID_KH = KhachHang.ID_KH and CTHD.ID_SP = SanPham.ID_SP and HoaDon.ID_HD = CTHD.ID_HD and HoaDon.ID_KH = KhachHang.ID_KH;
end 
EXEC sp_TKHD2 N'Nguyễn'
drop procedure sp_TKHD2

--procedure tìm kiếm hóa đơn theo tên sản phẩm--
CREATE PROCedure sp_TKHD3 @TenSP nvarchar(50)
as 
begin
	select HoaDon.ID_HD as [Mã hóa đơn] , HoaDon.ID_KH as [Mã khách] , KhachHang.Ten_KH as [Tên khách] , SanPham.Ten_SP as [Tên sản phẩm] , CTHD.SL_CTHD as [Số lượng mua], HoaDon.Ngay_HD as [Ngày mua hàng], CTHD.TT_CTHD as [Thành tiền], HoaDon.TT_HD as [Tổng tiền]
	from HoaDon , KhachHang , SanPham , CTHD 
	WHERE SanPham.Ten_SP like '%'+@TenSP+'%' and HoaDon.ID_KH = KhachHang.ID_KH and CTHD.ID_SP = SanPham.ID_SP and HoaDon.ID_HD = CTHD.ID_HD and HoaDon.ID_KH = KhachHang.ID_KH;
end 
EXEC sp_TKHD3 N'dao'
drop procedure sp_TKHD3

--procedure tìm kiếm khách hàng--
CREATE PROCedure sp_TKKH @TenK nvarchar(50)
as 
begin
	select ID_KH as [Mã khách hàng] , Ten_KH as [Tên khách] , DT_KH as [Điện thoại khách] , DC_KH as [Địa chỉ khách] , GioiTinh as [Giới tính]
	from KhachHang
	WHERE KhachHang.Ten_KH like '%'+@TenK+'%';
end 

EXEC sp_TKKH N'C'
drop procedure sp_TKKH
--View in ra bảng hóa đơn mua hàng 
create view v_HDMH 
as 
	select HoaDon.ID_HD as [Mã hóa đơn] , HoaDon.ID_KH as [Mã khách] , KhachHang.Ten_KH as [Tên khách] , SanPham.Ten_SP as [Tên sản phẩm] , CTHD.SL_CTHD as [Số lượng mua], HoaDon.Ngay_HD as [Ngày mua hàng], CTHD.TT_CTHD as [Thành tiền], HoaDon.TT_HD as [Tổng tiền]
	from HoaDon , KhachHang , SanPham , CTHD
	where HoaDon.ID_HD = CTHD.ID_HD and HoaDon.ID_KH = KhachHang.ID_KH and SanPham.ID_SP = CTHD.ID_SP

select * from v_HDMH
drop view v_HDMH

-- View NCC
create view v_NCC
as
select ID_NCC as [Mã NCC] , Ten_NCC as [Tên NCC] , DC_NCC as [Địa chỉ NCC] , DT_NCC as [Điện thoại NCC] , TT_GD as [Tình trạng giao dịch]
from NCC

select * from v_NCC
drop view v_NCC

-- View KH
create view v_KH
as
select ID_KH as [Mã khách hàng] , Ten_KH as [Tên khách] , DT_KH as [Điện thoại khách] , DC_KH as [Địa chỉ khách] , GioiTinh as [Giới tính]
from KhachHang

select * from v_KH
drop view v_KH

-- View SP (ID_SP,ID_NCC,Ten_SP,DGN_SP,SL_SP,DonGiaBan,MT_SP)
create view v_SP
as
select ID_SP as [Mã sản phẩm] , Ten_NCC as [Tên NCC] , Ten_SP as [Tên sản phẩm] , DGN_SP as [Đơn giá nhập] , SL_SP as [Số lượng sản phẩm] , DonGiaBan as [Đơn giá bán sản phẩm] , MT_SP as [Mô tả sản phẩm] , TT_SP as [Tình trạng sản phẩm]
from SanPham , NCC where SanPham.ID_NCC = NCC.ID_NCC

select * from v_SP
drop view v_SP

-- view TK
create view v_TK
as
select Tai_Khoan as [Tài khoản] , Mat_Khau as [Mật khẩu] , type as [Loại tài khoản]
from DangNhap

select * from v_Tk
drop view v_Tk
--   TRIGEER 1
--khi insert CTHD thì update SL_SP --
/* cập nhật hàng trong kho sau khi đặt hàng hoặc cập nhật */
create trigger trg_InsertCTHD on CTHD after insert 
as 
begin
	update SanPham SET SL_SP = SL_SP - (SELECT SL_CTHD FROM inserted WHERE inserted.ID_SP = SanPham.ID_SP)
	FROM SanPham, inserted where SanPham.ID_SP = inserted.ID_SP
END
GO


select Ten_SP , SL_SP from SanPham where ID_SP = 1
--insert into HoaDon (ID_HD , ID_KH , Ngay_HD) 
--values(6,1,'2021-09-11')
--insert into CTHD (ID_HD , ID_SP , SL_CTHD)
--values (6,1,5);
drop trigger trg_InsertCTHD
select * from CTHD

--  TRIGEER 4
--SLSP <= 0 thì trạng thái hết hàng 
create trigger trg_UpdateSLSP on SanPham for update
as 
begin
	declare @SLSP int , @SLSPCTHD int , @IDSP int ;
	select @SLSP = SL_SP from inserted
	select @IDSP = ID_SP from inserted
	select @SLSPCTHD = SL_SP from SanPham where SanPham.ID_SP = @IDSP
	if @SLSPCTHD < 0
		begin
			SET NOCOUNT ON; 
			RAISERROR (N'Số Lượng hàng đã hết , bạn không thể mua!' ,16,1)
			Rollback transaction
		end
	else if @SLSPCTHD = 0 
		begin
			update SanPham set TT_SP = N'Hết hàng!' where SanPham.ID_SP = @IDSP
		end
	else if @SLSPCTHD > 0 
		begin 
			update SanPham set TT_SP = N'Còn hàng' where SanPham.ID_SP = @IDSP
		end
end
go

select * from CTHD
select * from SanPham
drop trigger trg_UpdateSLSP
select Ten_SP , SL_SP , TT_SP from SanPham where ID_SP = 1
insert into CTHD (ID_HD , ID_SP , SL_CTHD)
values (6,1,115);



--------------Nhập vào ID hóa đơn trả về bảng gồm
-- id_HD, ID_Khach, tên khách, tên sản phẩm,số lượng sản phẩm và ngày mua hóa đơn 
go
CREATE function sp_TKHD1 (@MaHD int)
returns @bang2 table (ID_hd int, ID_KH int, tenKh nvarchar(50),Ten_sp nvarchar(50),Soluong int, Ngay date,TongtienSp float,TongTien_HD float ) 
as 
begin
	insert into @bang2
	SELECT HoaDon.ID_HD , HoaDon.ID_KH , KhachHang.Ten_KH , SanPham.Ten_SP , CTHD.SL_CTHD , HoaDon.Ngay_HD , CTHD.TT_CTHD,HoaDon.TT_HD  FROM HoaDon , CTHD , KhachHang , SanPham 
	WHERE HoaDon.ID_HD = @MaHD and HoaDon.ID_KH = KhachHang.ID_KH and CTHD.ID_SP = SanPham.ID_SP and HoaDon.ID_HD = CTHD.ID_HD ;
	return
end 
go
select * from  sp_TKHD1 (1)


-- PROCEDURE 1
-- Proc update thành tiền cho bảng CTHD
go
CREATE PROCEDURE sp_ThanhTien 
as
Begin 
	UPDATE CTHD SET CTHD.TT_CTHD = DonGiaBan * (SL_CTHD)
	FROM CTHD , SanPham 
	Where CTHD.ID_SP = SanPham.ID_SP;
	return
End
go

execute sp_ThanhTien 
drop proc sp_ThanhTien

-- PROCEDURE 2
-- Proc update tổng tiền cho bảng hóa đơn 
go
create proc HD_TongTien
as 
begin
	 update HoaDon
	 set TT_HD = (select sum(CTHD.TT_CTHD)from CTHD 
	 where HoaDon.ID_HD= CTHD.ID_HD )
	 return
end
go

exec HD_TongTien
drop proc HD_TongTien


-- TRIGGER 1 
-- Trigger tự động cập nhập thành tiền của bảng CTHD và tổng tiền của bảng hóa đơn khi nhập đủ 3 giá trị trong bảng CTHD
go
create trigger Isert_CTHD on CTHD for insert
as
	begin
		exec sp_ThanhTien 
		print N'Cập Nhật Thành Công'
		exec HD_TongTien
	end
go

drop trigger isert_CTHD

-- TRIGGER 2
-- Trigger thay vì xóa nhà cung cấp sẽ cập nhập tình trạng giao dịch thành ‘không còn giao dịch’ và tình trạng sản phẩm có nhà cung cấp bị xóa trong bảng sản phẩm là ‘Còn hàng nhưng không nhập thêm hàng mới’
create trigger trg_XoaNCC on NCC instead of delete
as
begin
	declare @IDNCC int
	set @IDNCC = (select ID_NCC from deleted)
	update NCC
	set TT_GD = N'Ngừng giao dịch'
	where ID_NCC = @IDNCC
	update SanPham
	set TT_SP = N'Còn hàng nhưng không cung cấp thêm hàng mới!'
	where SanPham.ID_NCC = @IDNCC
end

drop trigger trg_XoaNCC

