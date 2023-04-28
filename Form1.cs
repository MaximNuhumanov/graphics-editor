using System;
using System.Drawing;
using System.Windows.Forms;

namespace графический_редактор
{
    public partial class Form1 : Form
    {
        rectangle rec = new rectangle();
        ellipse el = new ellipse();
        line spline = new line();

        Boolean Рисовать_ли;
        Color c = Color.Red;
        Boolean Карандаш = false;
        Boolean Круг = false;
        Boolean Круг4 = false;
        Boolean Прямоугольник = false;
        Boolean Прямоугольник1 = false;
        Boolean Текст = false;
        Boolean линия = true;
        Boolean линия3 = false;
        Boolean линия2 = false;

        private Bitmap for_draw;
        string всп_картинка = System.IO.Directory.GetCurrentDirectory() + @"\pic.usp";
        string всп_картинка1 = System.IO.Directory.GetCurrentDirectory() + @"\pic1.usp";
        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            this.Text = "Рисунок";
            // Размеры формы
            // Создаем объект для работы с изображением
            Image Рисунок = (Image)new Bitmap(System.IO.Directory.GetCurrentDirectory() + @"\hand-painted-watercolor-background-with-sky-and-clouds-shape_24972-1095.png");
            // Вывод изображения в форму
            this.WindowState = FormWindowState.Maximized;
            e.Graphics.DrawImage(Zoom(Рисунок, (int)Math.Sqrt((this.Width * this.Height) / (Рисунок.Width * Рисунок.Height))+1), 0, 0);

        }
        Image Zoom(Image image, int k)
        {
            if (k <= 1) return image;
            Bitmap img = new Bitmap(image);
            int width = img.Width;
            int height = img.Height;
            Image zoomImg = new Bitmap(width * k, height * k);
            Graphics g = Graphics.FromImage(zoomImg);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    Color color = img.GetPixel(i, j);
                    g.FillRectangle(new SolidBrush(color), i * k, j * k, k, k);
                }

            return zoomImg;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

            Круг = !Круг;
            if (Круг == true)
                pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            if (Круг == false)
                pictureBox1.BorderStyle = BorderStyle.None;
        }
        // Булева переменная Рисовать_ли дает разрешение на рисование:
        private void PMouseDown(object sender, MouseEventArgs e)
        {
            // Если нажата кнопка мыши - MouseDown, то рисовать
            if (Текст)
            {
                using (Graphics g = Graphics.FromImage(for_draw))
                {
                    //рисуем линию
                    g.DrawString(
                        textBox1.Text,
                        Font, new SolidBrush(c), e.X, e.Y);
                }
                pictureBox2.Image = for_draw;
                pictureBox2.Invalidate();
                pictureBox6_Click(pictureBox6, EventArgs.Empty);
            }
            if (!линия)
            {
                PointF a = new PointF();
                a.X = e.X;
                a.Y = e.Y;
                spline.P1(a);
                линия = true;
            }
            if (Круг)
            {
                el.MDown(e);
            }
            if (Прямоугольник)
            {
                rec.MDown(e);
            }
            Рисовать_ли = true;
        }
        private void PMouseUp(object sender, MouseEventArgs e)
        {
            // Если кнопку мыши отпустили, то НЕ рисовать
            Рисовать_ли = false;
            if (Круг4)
            {
                el.MUP(for_draw, e, new Pen(c, 3));
                pictureBox2.Image = for_draw;
                pictureBox2.Invalidate();
                Круг4 = false;
            }
            if (Прямоугольник1)
            {
                rec.MUP(for_draw, e, new Pen(c, 3));
                pictureBox2.Image = for_draw;
                pictureBox2.Invalidate();
                Прямоугольник1 = false;
            }
            if (линия2 && линия3)
            {
                spline.Paint(for_draw, e, new Pen(c, 3));
                pictureBox2.Image = for_draw;
                pictureBox2.Invalidate();
                линия = false;
            }
        }
        private void PMouseMove(object sender, MouseEventArgs e)
        {
            // Рисование прямоугольника, если нажата кнопка мыши

            if (Рисовать_ли == true && Карандаш == true)
            {
                using (Graphics g = Graphics.FromImage(for_draw))
                {
                    //рисуем линию
                    g.FillRectangle(new SolidBrush(c), e.X, e.Y, 10, 10);

                    pictureBox2.Image = for_draw;
                    pictureBox2.Invalidate();

                }
            }
            if (Рисовать_ли == true && Круг == true)
            {

                el.MMove(e);
                Круг4 = true;
            }
            if (Рисовать_ли == true && Прямоугольник == true)
            {

                rec.MMove(e);
                Прямоугольник1 = true;
            }
            if (Рисовать_ли == true && линия == true && линия3)
            {
                PointF a1 = new PointF();
                a1.X = e.X;
                a1.Y = e.Y;
                spline.p4(a1);
                линия2 = true;
            }

        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Карандаш = !Карандаш;
            if (Карандаш == true)
                pictureBox3.BorderStyle = BorderStyle.Fixed3D;
            if (Карандаш == false)
                pictureBox3.BorderStyle = BorderStyle.None;



        }
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var image = for_draw;
            if (pictureBox1.Image != null) //если в pictureBox есть изображение
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog Opendialog = new OpenFileDialog();
            Opendialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*"; //формат загружаемого файла
            if (Opendialog.ShowDialog() == DialogResult.OK) //если в окне была нажата кнопка "ОК"
            {
                try
                {
                    var image = new Bitmap(Opendialog.FileName);
                    //вместо pictureBox1 укажите pictureBox, в который нужно загрузить изображение 
                    panel1.Visible = true;
                    pictureBox2.Visible = true;
                    for_draw = image;
                    pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
                    pictureBox2.Image = image;
                    panel1.Size = new Size(pictureBox2.Width, 750);
                    pictureBox2.Invalidate();
                }
                catch
                {
                    DialogResult rezult = MessageBox.Show("Невозможно открыть выбранный файл",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void Поле(object sender, EventArgs e)
        {

        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void pic4(object sender, EventArgs e)
        {
            линия3 = !линия3;
            линия = false;
            if (линия3 == true)
                pictureBox4.BorderStyle = BorderStyle.Fixed3D;
            if (линия3 == false)
                pictureBox4.BorderStyle = BorderStyle.None;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.Text == "Transparent") return;
            c = Color.FromName(comboBox1.Text);
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Прямоугольник = !Прямоугольник;
            if (Прямоугольник == true)
                pictureBox5.BorderStyle = BorderStyle.Fixed3D;
            if (Прямоугольник == false)
                pictureBox5.BorderStyle = BorderStyle.None;
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Текст = !Текст;
            textBox1.Visible = !textBox1.Visible;
            if (Текст == true)
                pictureBox6.BorderStyle = BorderStyle.Fixed3D;
            if (Текст == false)
                pictureBox6.BorderStyle = BorderStyle.None;
        }
        private void comboBox1_Enter(object sender, EventArgs e)
        {

            // Получаем массив строк имен цветов из перечисления KnownColor
            var ВсеЦвета = Enum.GetNames(typeof(KnownColor));
            // В VB: Dim ВсеЦвета = [Enum].GetNames(GetType(KnownColor))
            comboBox1.Items.Clear();
            // В список элементов добавляем имена всех цветов:
            comboBox1.Items.AddRange(ВсеЦвета);
            // Сортировать записи в алфавитном порядке
            comboBox1.Sorted = true;
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {


            var image = new Bitmap(System.IO.Directory.GetCurrentDirectory() +
                                               @"\белый_лист.usp");
            //вместо pictureBox1 укажите pictureBox, в который нужно загрузить изображение 
            panel1.Visible = true;
            pictureBox2.Visible = true;
            for_draw = image;
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.Image = image;
            panel1.Size = new Size(pictureBox2.Width, 750);
            pictureBox2.Invalidate();
        }
    }
}
class line
{
    // Начальная узловая точка:  
    PointF p0;
    // Конечная узловая точка:
    PointF p3;
    public void P1(PointF a1)
    { p0 = a1; }
    public void p4(PointF a4)
    { p3 = a4; }
    public void Paint(Bitmap for_draw, MouseEventArgs e, Pen Перо)
    {
        using (Graphics g = Graphics.FromImage(for_draw))
        {
            // Рисуем начальную и конечную узловые точки диаметром 4 пикселя:
            g.DrawLine(Перо, p0.X, p0.Y, p3.X, p3.Y);
            g.Dispose();
        }
    }
};
class ellipse
{
    Point Круг_верх;
    Point Круг_низ;
    public void MUP(Bitmap for_draw, MouseEventArgs e, Pen Перо)
    {

        using (Graphics g = Graphics.FromImage(for_draw))
        {
            //рисуем линию
            g.DrawEllipse(Перо, Круг_верх.X, Круг_верх.Y, Круг_низ.X, Круг_низ.Y);
        }

    }
    public void MDown(MouseEventArgs e)
    {
        Круг_верх.X = e.X;
        Круг_верх.Y = e.Y;
    }
    public void MMove(MouseEventArgs e)
    {

        Круг_низ.X = e.X - Круг_верх.X;
        Круг_низ.Y = e.Y - Круг_верх.Y;
    }
}
class rectangle
{
    Point угол_верх;
    Point угол_низ;
    public void MUP(Bitmap for_draw, MouseEventArgs e, Pen Перо)
    {

        using (Graphics g = Graphics.FromImage(for_draw))
        {
            //рисуем линию
            g.DrawRectangle(Перо, угол_верх.X, угол_верх.Y, угол_низ.X, угол_низ.Y);
        }

    }
    public void MDown(MouseEventArgs e)
    {
        угол_верх.X = e.X;
        угол_верх.Y = e.Y;
    }
    public void MMove(MouseEventArgs e)
    {

        угол_низ.X = e.X - угол_верх.X;
        угол_низ.Y = e.Y - угол_верх.Y;
        //рисуем линию
    }
}
