using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClothesBadmintonManagent
{
    public partial class Form5 : Form
    {
        // Chuỗi kết nối đến cơ sở dữ liệu
        string connectstring = @"Data Source=Truong_Ngoc_Hoa;Initial Catalog=Highs;Integrated Security=True;TrustServerCertificate=True";

        SqlConnection con; // Đối tượng kết nối SQL
        SqlCommand cmd; // Đối tượng lệnh SQL
        SqlDataAdapter adt; // Đối tượng để lấy dữ liệu từ DB
        DataTable dt = new DataTable(); // Bảng dữ liệu để lưu trữ sản phẩm
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            con = new SqlConnection(connectstring); // Khởi tạo kết nối với cơ sở dữ liệu
            try
            {
                con.Open(); // Mở kết nối
                cmd = new SqlCommand("select ProductID, ProductName, InputPrice, SellingPrice, Quantity from Products", con); // Tạo lệnh SQL để lấy tất cả sản phẩm
                adt = new SqlDataAdapter(cmd); // Tạo SqlDataAdapter từ lệnh
                adt.Fill(dt); // Điền dữ liệu vào DataTable
                GridV_hienthi6.DataSource = dt; // Gán DataTable cho DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hiển thị thông báo lỗi nếu có
            }
            finally
            {
                con.Close(); // Đóng kết nối
            }
        }

        private void GridV_hienthi6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra nếu chỉ số dòng hợp lệ
            {
                DataGridViewRow selectedRow = GridV_hienthi6.Rows[e.RowIndex];

                //try
                //{
                //    // Gán giá trị từ dòng được chọn vào các TextBox tương ứng
                //    txtid.Text = selectedRow.Cells["ProductID"].Value.ToString();       // ProductID
                //    txtname.Text = selectedRow.Cells["ProductName"].Value.ToString();   // ProductName

                //    // Nếu cột ProductImage chứa giá trị không rỗng, chuyển đổi sang hình ảnh
                //    if (selectedRow.Cells["ProductImage"].Value != DBNull.Value)
                //    {
                //        byte[] imageData = (byte[])selectedRow.Cells["ProductImage"].Value;
                //        if (imageData != null && imageData.Length > 0)
                //        {
                //            using (MemoryStream ms = new MemoryStream(imageData))
                //            {
                //                picB_imageProduct.Image = Image.FromStream(ms);
                //            }
                //        }
                //        else
                //        {
                //            picB_imageProduct.Image = null;
                //        }
                //    }
                //    else
                //    {
                //        picB_imageProduct.Image = null;
                //    }

                    // Gán giá trị cho các cột còn lại
                    txtid.Text = selectedRow.Cells["ProductID"].Value?.ToString();    // SizeProduct
                    txtname.Text = selectedRow.Cells["ProductName"].Value.ToString();    // InputPrice
                    txtentry.Text = selectedRow.Cells["InputPrice"].Value.ToString(); // InventoryPrice
                    txtsell.Text = selectedRow.Cells["SellingPrice"].Value.ToString();   // SellingPrice
                    txtqtt.Text = selectedRow.Cells["Quantity"].Value.ToString();        // Quantity
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message); // Hiển thị thông báo lỗi nếu có
                //}
            }
        }


        private void btn_addProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(txtid.Text, out int productId))
                {
                    MessageBox.Show("Product ID must be a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                con = new SqlConnection(connectstring);
                con.Open();
                string checkQuery = "SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID";
                SqlCommand cmdCheck = new SqlCommand(checkQuery, con);
                cmdCheck.Parameters.AddWithValue("@ProductID", txtid.Text);

                int count = (int)cmdCheck.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("This product ID already exists, please choose a different ID.", "Duplicate ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                    return;
                }
                string insertQuery = "INSERT INTO Products(ProductID, ProductName, InputPrice, SellingPrice, Quantity) " +
                    "VALUES(@ProductID, @ProductName, @Input_Price, @Selling_Price, @Quantity)";
                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@ProductID", txtid.Text);
                cmd.Parameters.AddWithValue("@ProductName", txtname.Text);
                cmd.Parameters.AddWithValue("@Input_Price", txtentry.Text);
                cmd.Parameters.AddWithValue("@Selling_Price", txtsell.Text);
                cmd.Parameters.AddWithValue("@Quantity", txtqtt.Text);
                cmd.ExecuteNonQuery();
                //Loaddata();
                MessageBox.Show("Product added successfully.");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //// Kiểm tra tính hợp lệ của dữ liệu đầu vào
            //if (string.IsNullOrWhiteSpace(txtB_idProduct.Text) || // Kiểm tra ProductID
            //    string.IsNullOrWhiteSpace(txtB_nameProduct.Text) || // Kiểm tra ProductName
            //    //string.IsNullOrWhiteSpace(cbB_sizeProduct.Text) || // Kiểm tra SizeProduct
            //    !decimal.TryParse(txtB_inputPrice.Text, out decimal InputPrice) || // Kiểm tra InputPrice
            //    !decimal.TryParse(txtB_inventProduct.Text, out decimal InventoryPrice) || // Kiểm tra InventoryPrice
            //    !decimal.TryParse(txtB_priceProduct.Text, out decimal SellingPrice)) // Kiểm tra SellingPrice
            //{
            //    MessageBox.Show("Please fill in all fields with valid data.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return; // Thoát nếu dữ liệu không hợp lệ
            //}

            //// Chuyển đổi đường dẫn hình ảnh thành mảng byte
            //byte[] productImage = PathToByteArray(this.Text); // Đảm bảo this.Text chứa đường dẫn hình ảnh hoặc sửa thành đường dẫn cụ thể.

            // Kết nối cơ sở dữ liệu
            using (SqlConnection con = new SqlConnection(connectstring))
            {
                try
                {
                    con.Open(); // Mở kết nối

                    // Tạo lệnh SQL INSERT để thêm sản phẩm
                    string query = @"INSERT INTO Products (ProductID, ProductName, ProductImage, InputPrice, SellingPrice, Quantity) 
                             VALUES (@ProductID, @ProductName, @ProductImage, @InputPrice, @SellingPrice, @Quantity)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Gán tham số cho câu lệnh SQL
                        //cmd.Parameters.AddWithValue("@ProductID", int.Parse(txtB_idProduct.Text)); // Lấy ProductID
                        //cmd.Parameters.AddWithValue("@ProductName", txtB_nameProduct.Text); // Lấy ProductName
                        ////cmd.Parameters.AddWithValue("@SizeProduct", cbB_sizeProduct.Text); // Lấy SizeProduct
                        //cmd.Parameters.AddWithValue("@ProductImage", productImage); // Lấy ProductImage
                        //cmd.Parameters.AddWithValue("@InputPrice", InputPrice); // Lấy InputPrice
                        //cmd.Parameters.AddWithValue("@InventoryPrice", InventoryPrice); // Lấy InventoryPrice
                        //cmd.Parameters.AddWithValue("@SellingPrice", SellingPrice); // Lấy SellingPrice

                        //cmd.ExecuteNonQuery(); // Thực thi câu lệnh INSERT
                    }

                    // Thông báo thành công
                    MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tải lại danh sách sản phẩm
                    LoadProducts();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        // Chuyển đổi file hình ảnh thành mảng byte
        byte[] PathToByteArray(string path)
        {
            MemoryStream m = new MemoryStream(); // Tạo MemoryStream để lưu trữ hình ảnh
            Image img = Image.FromFile(path); // Tải hình ảnh từ file
            img.Save(m, System.Drawing.Imaging.ImageFormat.Png); // Lưu hình ảnh vào MemoryStream
            return m.ToArray(); // Trả về mảng byte
        }

        private void LoadProducts()
        {
            dt.Clear(); // Xóa dữ liệu cũ trong DataTable
            using (SqlConnection con = new SqlConnection(connectstring)) // Tạo kết nối mới
            {
                try
                {
                    con.Open(); // Mở kết nối
                    cmd = new SqlCommand("SELECT * FROM Products", con); // Tạo lệnh SQL để lấy tất cả sản phẩm
                    adt = new SqlDataAdapter(cmd); // Tạo SqlDataAdapter từ lệnh
                    adt.Fill(dt); // Điền dữ liệu vào DataTable
                    GridV_hienthi6.DataSource = dt; // Gán DataTable cho DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message); // Hiển thị thông báo lỗi nếu có
                }
            }
        }

        private void picB_imageProduct_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog(); // Tạo hộp thoại mở file
            if (open.ShowDialog() == DialogResult.OK) // Kiểm tra xem người dùng đã chọn file chưa
            {
                //picB_imageProduct.Image = Image.FromFile(open.FileName); // Hiển thị hình ảnh đã chọn
                this.Text = open.FileName; // Hiển thị đường dẫn file cho mục đích gỡ lỗi
            }
        }
        private void btn_updProduct_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(connectstring);
            con.Open();
            adt = new SqlDataAdapter(cmd);
            string edit = ("update Products set ProductID = @ProductID, ProductName = @ProductName, InputPrice = @Input_Price," +
                " SellingPrice = @Selling_Price, Quantity = @Quantity where ProductID = @ProductID");
            cmd = new SqlCommand(edit, con);
            cmd.Parameters.AddWithValue("@ProductID", txtid.Text);
            cmd.Parameters.AddWithValue("@ProductName", txtname.Text);
            cmd.Parameters.AddWithValue("@Input_Price", txtentry.Text);
            cmd.Parameters.AddWithValue("@Selling_Price", txtsell.Text);
            cmd.Parameters.AddWithValue("@Quantity", txtqtt.Text);
            cmd.ExecuteNonQuery();
            //Loaddata();
            MessageBox.Show("More Success");
            con.Close();
            con = new SqlConnection(connectstring); // Khởi tạo kết nối với cơ sở dữ liệu
            try
            {
                con.Open(); // Mở kết nối
                cmd = new SqlCommand("select ProductID, ProductName, InputPrice, SellingPrice, Quantity from Products", con); // Tạo lệnh SQL để lấy tất cả sản phẩm
                adt = new SqlDataAdapter(cmd); // Tạo SqlDataAdapter từ lệnh
                adt.Fill(dt); // Điền dữ liệu vào DataTable
                GridV_hienthi6.DataSource = dt; // Gán DataTable cho DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hiển thị thông báo lỗi nếu có
            }
            finally
            {
                con.Close(); // Đóng kết nối
            }
           
        }
        

        private void btn_deleProduct_Click(object sender, EventArgs e)
        {
            if (GridV_hienthi6.SelectedRows.Count > 0) // Kiểm tra xem có dòng nào được chọn không
            {
                // Lấy dòng được chọn
                var selectedRow = GridV_hienthi6.SelectedRows[0];
                int productIdToDelete = Convert.ToInt32(selectedRow.Cells["ProductID"].Value); // Lấy ProductID từ dòng đã chọn

                // Xác nhận trước khi xóa
                var confirmResult = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    using (SqlConnection con = new SqlConnection(connectstring)) // Tạo kết nối mới
                    {
                        try
                        {
                            con.Open(); // Mở kết nối

                            // Xóa các bản ghi liên quan trong các bảng khác nếu có
                            string deleteRelatedQuery = "DELETE FROM OrderDetails WHERE ProductID = @ProductID";
                            using (SqlCommand cmdRelated = new SqlCommand(deleteRelatedQuery, con))
                            {
                                cmdRelated.Parameters.AddWithValue("@ProductID", productIdToDelete);
                                cmdRelated.ExecuteNonQuery();
                            }

                            // Tạo câu lệnh DELETE để xóa sản phẩm
                            string deleteProductQuery = "DELETE FROM Products WHERE ProductID = @ProductID";
                            using (SqlCommand cmdProduct = new SqlCommand(deleteProductQuery, con))
                            {
                                cmdProduct.Parameters.AddWithValue("@ProductID", productIdToDelete); // Thêm tham số ProductID
                                int rowsAffected = cmdProduct.ExecuteNonQuery(); // Thực thi câu lệnh DELETE

                                if (rowsAffected > 0)
                                {
                                    // Xóa dòng khỏi DataGridView sau khi xóa thành công trong cơ sở dữ liệu
                                    GridV_hienthi6.Rows.RemoveAt(selectedRow.Index);
                                    MessageBox.Show("Product deleted successfully!");
                                }
                                else
                                {
                                    MessageBox.Show("No product found with the given ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show($"SQL Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Hiển thị lỗi cơ sở dữ liệu nếu có
                        }
                        finally
                        {
                            con.Close(); // Đóng kết nối
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete."); // Thông báo nếu không có dòng nào được chọn
            }
        }


        private void btn_search_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtsr.Text, out int productId))
            {
                MessageBox.Show("Please enter a valid product ID (numbers only).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            con = new SqlConnection(connectstring);
            con.Open();
            cmd = new SqlCommand("Select ProductID, ProductName, InputPrice, SellingPrice, Quantity from Products where ProductID = @Product_ID", con);
            cmd.Parameters.AddWithValue("@Product_ID", txtsr.Text);
            cmd.ExecuteNonQuery();
            adt = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adt.Fill(dt);
            GridV_hienthi6.DataSource = dt;
            con.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void GridV_hienthi6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtid_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(connectstring); // Khởi tạo kết nối với cơ sở dữ liệu
            try
            {
                con.Open(); // Mở kết nối
                cmd = new SqlCommand("select ProductID, ProductName, InputPrice, SellingPrice, Quantity from Products", con); // Tạo lệnh SQL để lấy tất cả sản phẩm
                adt = new SqlDataAdapter(cmd); // Tạo SqlDataAdapter từ lệnh
                adt.Fill(dt); // Điền dữ liệu vào DataTable
                GridV_hienthi6.DataSource = dt; // Gán DataTable cho DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Hiển thị thông báo lỗi nếu có
            }
            finally
            {
                con.Close(); // Đóng kết nối
            }
        }
    }
}
