using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext.Enums
{
    public enum EnumStatus
    {
        /// <summary>
        /// Транзакция была отменена
        /// </summary>
        Canceled,
        /// <summary>
        /// Обработка транзакии
        /// </summary>
        Processing,
        /// <summary>
        /// Транзакция требует дополнительных действий со стороны клиента.
        /// </summary>
        RequiresAction,
        /// <summary>
        /// Транзакция подтверждена и требует захвата.
        /// </summary>
        QuiresCapture,
        /// <summary>
        /// Требуется подтверждение
        /// </summary>
        RequiresConfirmation,
        /// <summary>
        /// Требуется присоединение способа оплаты.
        /// </summary>
        RequiresPaymentMethod,
        /// <summary>
        /// Успешное выполнение
        /// </summary>
        Succeeded
    }
}
