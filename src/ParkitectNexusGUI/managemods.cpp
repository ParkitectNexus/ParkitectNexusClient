#include "managemods.h"
#include "ui_managemods.h"

ManageMods::ManageMods(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::ManageMods)
{
    ui->setupUi(this);
}

ManageMods::~ManageMods()
{
    delete ui;
}
