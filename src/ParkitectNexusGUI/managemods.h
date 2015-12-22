#ifndef MANAGEMODS_H
#define MANAGEMODS_H

#include <QDialog>

namespace Ui {
class ManageMods;
}

class ManageMods : public QDialog
{
    Q_OBJECT

public:
    explicit ManageMods(QWidget *parent = 0);
    ~ManageMods();

private:
    Ui::ManageMods *ui;
};

#endif // MANAGEMODS_H
