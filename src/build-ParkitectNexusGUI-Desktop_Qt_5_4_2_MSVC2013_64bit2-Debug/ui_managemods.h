/********************************************************************************
** Form generated from reading UI file 'managemods.ui'
**
** Created by: Qt User Interface Compiler version 5.4.2
**
** WARNING! All changes made in this file will be lost when recompiling UI file!
********************************************************************************/

#ifndef UI_MANAGEMODS_H
#define UI_MANAGEMODS_H

#include <QtCore/QVariant>
#include <QtWidgets/QAction>
#include <QtWidgets/QApplication>
#include <QtWidgets/QButtonGroup>
#include <QtWidgets/QDialog>
#include <QtWidgets/QHeaderView>
#include <QtWidgets/QListWidget>
#include <QtWidgets/QPushButton>

QT_BEGIN_NAMESPACE

class Ui_ManageMods
{
public:
    QListWidget *listWidget;
    QPushButton *pushButton;

    void setupUi(QDialog *ManageMods)
    {
        if (ManageMods->objectName().isEmpty())
            ManageMods->setObjectName(QStringLiteral("ManageMods"));
        ManageMods->resize(598, 419);
        listWidget = new QListWidget(ManageMods);
        listWidget->setObjectName(QStringLiteral("listWidget"));
        listWidget->setGeometry(QRect(20, 20, 351, 351));
        pushButton = new QPushButton(ManageMods);
        pushButton->setObjectName(QStringLiteral("pushButton"));
        pushButton->setGeometry(QRect(420, 350, 75, 23));

        retranslateUi(ManageMods);

        QMetaObject::connectSlotsByName(ManageMods);
    } // setupUi

    void retranslateUi(QDialog *ManageMods)
    {
        ManageMods->setWindowTitle(QApplication::translate("ManageMods", "Dialog", 0));
        pushButton->setText(QApplication::translate("ManageMods", "PushButton", 0));
    } // retranslateUi

};

namespace Ui {
    class ManageMods: public Ui_ManageMods {};
} // namespace Ui

QT_END_NAMESPACE

#endif // UI_MANAGEMODS_H
